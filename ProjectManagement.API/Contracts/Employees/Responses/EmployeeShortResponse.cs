namespace ProjectManagement.API.Contracts.Employees.Responses;

public sealed record EmployeeShortResponse
{
    public required Guid Id { get; init; }

    public required string FullName { get; init; }
}