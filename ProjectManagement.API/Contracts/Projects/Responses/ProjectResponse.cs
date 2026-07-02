using ProjectManagement.API.Contracts.Employees.Responses;

namespace ProjectManagement.API.Contracts.Projects.Responses;

public sealed record ProjectResponse
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string CustomerCompany { get; init; }

    public required string ExecutorCompany { get; init; }

    public required DateOnly StartDate { get; init; }

    public required DateOnly EndDate { get; init; }

    public required int Priority { get; init; }

    public required EmployeeShortResponse Manager { get; init; }
}