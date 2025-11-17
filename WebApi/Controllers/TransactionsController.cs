using Application.Contract.Event;
using Application.DTOs;
using Application.Service;
using Microsoft.AspNetCore.Mvc;
using MassTransit;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;
        private readonly IBus _bus;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ITransactionService service,IBus bus, ILogger<TransactionsController> logger)
        {
            _service = service;
            _bus = bus;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            var transaction = await _service.GetTransactionByIdAsync(id);
            if (transaction == null) return NotFound();
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _service.CreateTransactionAsync(dto);
            var message = new TransactionPendingEvent(result.Id, result.Amount);
            await _bus.Publish(message);
            _logger.LogInformation("Transaction {TransactionId} created for User {UserId}", result.Id, result.UserId);
            return CreatedAtAction(nameof(GetTransactionById), new { id = result.Id }, result);
        }
        
        [HttpGet("stats")]
        public async Task<IActionResult> GetDailyStats()
        {
            var stats = await _service.GetDailyStatsAsync();
            return Ok(stats);
        }
    }
}