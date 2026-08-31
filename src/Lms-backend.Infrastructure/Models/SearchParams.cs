namespace Lms_backend.Infrastructure.Models;

public class SearchParams(string? name, string? search)
{
    public string? Name { get; init; } = name;
    public string? Search { get; init; } = search;
}
