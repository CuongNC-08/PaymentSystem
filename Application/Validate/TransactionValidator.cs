using Application.DTOs;
using Domain.Entity;
using FluentValidation;

namespace Application.Validate;

public class TransactionValidator : AbstractValidator<TransactionCreateDto>
{
    public TransactionValidator()
    {
        RuleFor(t => t.UserId)
            .GreaterThan(0).WithMessage("UserId phải lớn hơn 0");

        RuleFor(t => t.Amount)
            .GreaterThan(0).WithMessage("Amount phải lớn hơn 0");
    }
}