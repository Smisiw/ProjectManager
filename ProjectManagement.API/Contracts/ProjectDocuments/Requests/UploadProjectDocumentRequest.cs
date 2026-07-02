namespace ProjectManagement.API.Contracts.ProjectDocuments.Requests;

public sealed record UploadProjectDocumentRequest
{
    public required IFormFile File { get; init; }
}