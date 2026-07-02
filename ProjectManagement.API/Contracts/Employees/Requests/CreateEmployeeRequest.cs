namespace ProjectManagement.API.Contracts.Employees.Requests;

public sealed record CreateEmployeeRequest
{
    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public string? MiddleName { get; init; }

    public required string Email { get; init; }
}