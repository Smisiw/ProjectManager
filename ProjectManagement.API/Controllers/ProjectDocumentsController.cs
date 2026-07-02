using Microsoft.AspNetCore.Mvc;
using ProjectManagement.API.Contracts.ProjectDocuments.Responses;
using ProjectManagement.API.Services.ProjectDocuments;

namespace ProjectManagement.API.Controllers;

[ApiController]
[Route("api/projects/{projectId:guid}/documents")]
[Produces("application/json")]
public sealed class ProjectDocumentsController(
    IProjectDocumentService projectDocumentService)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<ProjectDocumentResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyCollection<ProjectDocumentResponse>>> GetAll(
        Guid projectId,
        CancellationToken cancellationToken)
    {
        var documents = await projectDocumentService.GetByProjectIdAsync(
            projectId,
            cancellationToken);

        return Ok(documents);
    }

    [HttpPost]
    [ProducesResponseType<ProjectDocumentResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectDocumentResponse>> Upload(
        Guid projectId,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        var document = await projectDocumentService.UploadAsync(
            projectId,
            file,
            cancellationToken);

        return CreatedAtAction(
            nameof(Download),
            new
            {
                projectId,
                documentId = document.Id
            },
            document);
    }

    [HttpGet("{documentId:guid}")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(
        Guid projectId,
        Guid documentId,
        CancellationToken cancellationToken)
    {
        var (stream, fileName, contentType) =
            await projectDocumentService.DownloadAsync(
                projectId,
                documentId,
                cancellationToken);

        return File(stream, contentType, fileName);
    }

    [HttpDelete("{documentId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid projectId,
        Guid documentId,
        CancellationToken cancellationToken)
    {
        await projectDocumentService.DeleteAsync(
            projectId,
            documentId,
            cancellationToken);

        return NoContent();
    }
}