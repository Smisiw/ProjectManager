namespace ProjectManagement.API.Contracts.Employees.Responses;

public sealed record EmployeeResponse
{
    public required Guid Id { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public string? MiddleName { get; init; }

    public required string Email { get; init; }
}