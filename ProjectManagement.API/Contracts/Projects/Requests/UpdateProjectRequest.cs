namespace ProjectManagement.API.Contracts.Projects.Requests;

public sealed record UpdateProjectRequest
{
    public required string Name { get; init; }

    public required string CustomerCompany { get; init; }

    public required string ExecutorCompany { get; init; }

    public required DateOnly StartDate { get; init; }

    public required DateOnly EndDate { get; init; }

    public required int Priority { get; init; }

    public required Guid ManagerId { get; init; }

    public IReadOnlyCollection<Guid> EmployeeIds { get; init; } = [];
}