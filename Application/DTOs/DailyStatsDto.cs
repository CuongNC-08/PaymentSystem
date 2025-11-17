namespace Application.DTOs;

public record DailyStatsDto
{
    public int SuccessCount { get; set; }
    public int FailedCount { get; set; }
}