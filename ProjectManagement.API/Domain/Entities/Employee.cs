namespace ProjectManagement.API.Domain.Entities;

public class Employee
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? MiddleName { get; set; }

    public required string Email { get; set; }

    public ICollection<Project> ManagedProjects { get; } = [];

    public ICollection<Project> Projects { get; } = [];
}