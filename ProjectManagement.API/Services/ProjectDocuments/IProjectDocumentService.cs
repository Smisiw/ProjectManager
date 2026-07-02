using ProjectManagement.API.Contracts.ProjectDocuments.Responses;

namespace ProjectManagement.API.Services.ProjectDocuments;

public interface IProjectDocumentService
{
    Task<ProjectDocumentResponse> UploadAsync(
        Guid projectId,
        IFormFile file,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ProjectDocumentResponse>> GetByProjectIdAsync(
        Guid projectId,
        CancellationToken cancellationToken = default);

    Task<(Stream Stream, string FileName, string ContentType)> DownloadAsync(
        Guid projectId,
        Guid documentId,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid projectId,
        Guid documentId,
        CancellationToken cancellationToken = default);
}