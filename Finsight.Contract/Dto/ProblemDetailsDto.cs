namespace Finsight.Contract.Dto;

public sealed class ProblemDetailsDto
{
    public string? Type { get; set; }

    public string? Title { get; set; }

    public string? Detail { get; set; }

    public int? Status { get; set; }

    public string? Instance { get; set; }
}
