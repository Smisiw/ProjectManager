using ProjectManagement.API.Contracts.Projects.Enums;

namespace ProjectManagement.API.Contracts.Projects.Requests;

public sealed record GetProjectsRequest
{
    public string? Search { get; init; }

    public int? Priority { get; init; }

    public Guid? ManagerId { get; init; }

    public DateOnly? StartDateFrom { get; init; }

    public DateOnly? StartDateTo { get; init; }

    public ProjectSortField? SortBy { get; init; }

    public bool Descending { get; init; }
}