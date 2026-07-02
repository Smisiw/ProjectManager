using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.API.Contracts.ProjectDocuments.Responses;
using ProjectManagement.API.Data;
using ProjectManagement.API.Domain.Entities;
using ProjectManagement.API.Exceptions;

namespace ProjectManagement.API.Services.ProjectDocuments;

public class ProjectDocumentService(
    AppDbContext context,
    IMapper mapper,
    IWebHostEnvironment environment) : IProjectDocumentService
{
    public async Task<ProjectDocumentResponse> UploadAsync(
        Guid projectId,
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        var project = await context.Projects
            .FindAsync([projectId], cancellationToken);
        if (project is null)
        {
            throw new NotFoundException(nameof(Project), projectId);
        }
        var relativeDirectory = Path.Combine(
            "Files",
            "Projects",
            projectId.ToString());

        var uploadsDirectory = Path.Combine(
            environment.ContentRootPath,
            relativeDirectory);

        Directory.CreateDirectory(uploadsDirectory);

        var extension = Path.GetExtension(file.FileName);
        var storedFileName = $"{Guid.CreateVersion7()}{extension}";

        var relativePath = Path.Combine(relativeDirectory, storedFileName);
        var fullPath = Path.Combine(environment.ContentRootPath, relativePath);
        await using (var stream = File.Create(fullPath))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }
        var document = new ProjectDocument
        {
            Id = Guid.CreateVersion7(),
            FileName = file.FileName,
            FilePath = relativePath,
            ContentType = file.ContentType,
            Size = file.Length,
            ProjectId = projectId
        };

        context.ProjectDocuments.Add(document);

        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<ProjectDocumentResponse>(document);
    }

    public async Task<IReadOnlyCollection<ProjectDocumentResponse>> GetByProjectIdAsync(
        Guid projectId,
        CancellationToken cancellationToken = default)
    {
        var project = await context.Projects
            .FindAsync([projectId], cancellationToken);

        if (project is null)
            throw new NotFoundException(nameof(Project), projectId);

        var documents = await context.ProjectDocuments
            .Where(d => d.ProjectId == projectId)
            .OrderBy(d => d.FileName)
            .ToListAsync(cancellationToken);

        return mapper.Map<IReadOnlyCollection<ProjectDocumentResponse>>(documents);
    }

    public async Task<(Stream Stream, string FileName, string ContentType)> DownloadAsync(
        Guid projectId,
        Guid documentId,
        CancellationToken cancellationToken = default)
    {
        var document = await context.ProjectDocuments
                           .FirstOrDefaultAsync(d => 
                               d.Id == documentId &&
                               d.ProjectId == projectId,
                               cancellationToken)
                       ?? throw new NotFoundException(nameof(ProjectDocument), documentId);

        var fullPath = Path.Combine(
            environment.ContentRootPath,
            document.FilePath);
        if (!File.Exists(fullPath))
            throw new NotFoundException("File not found.");

        var stream = File.OpenRead(document.FilePath);

        return (
            stream,
            document.FileName,
            document.ContentType);
    }

    public async Task DeleteAsync(
        Guid projectId,
        Guid documentId,
        CancellationToken cancellationToken = default)
    {
        var document = await context.ProjectDocuments
                           .FirstOrDefaultAsync(d => 
                               d.Id == documentId &&
                               d.ProjectId == projectId,
                               cancellationToken)
                       ?? throw new NotFoundException(nameof(ProjectDocument), documentId);

        var fullPath = Path.Combine(
            environment.ContentRootPath,
            document.FilePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        context.ProjectDocuments.Remove(document);

        await context.SaveChangesAsync(cancellationToken);
    }
}