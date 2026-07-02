namespace ProjectManagement.API.Domain.Entities;

public class ProjectDocument
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public required string FileName { get; set; }

    public required string FilePath { get; set; }
    
    public required string ContentType { get; set; }

    public required long Size { get; set; }

    public Guid ProjectId { get; set; }

    public Project Project { get; set; } = null!;
}