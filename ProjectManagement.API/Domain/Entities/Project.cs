namespace ProjectManagement.API.Domain.Entities;

public class Project
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public required string Name { get; set; }

    public required string CustomerCompany { get; set; }

    public required string ExecutorCompany { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int Priority { get; set; }

    public Guid ManagerId { get; set; }

    public Employee Manager { get; set; } = null!;

    public ICollection<Employee> Employees { get; } = [];

    public ICollection<ProjectDocument> Documents { get; } = [];
}