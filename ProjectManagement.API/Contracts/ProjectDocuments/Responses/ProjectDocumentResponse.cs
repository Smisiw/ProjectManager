namespace ProjectManagement.API.Contracts.ProjectDocuments.Responses;


public sealed record ProjectDocumentResponse
{
    public required Guid Id { get; init; }

    public required string FileName { get; init; }
    
    public required long Size { get; init; }
    
    public required string ContentType { get; init; }
}