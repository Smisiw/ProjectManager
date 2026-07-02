using Microsoft.AspNetCore.Mvc;
using ProjectManagement.API.Contracts.Projects.Requests;
using ProjectManagement.API.Contracts.Projects.Responses;
using ProjectManagement.API.Services.Projects;

namespace ProjectManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class ProjectsController(IProjectService projectService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<ProjectResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ProjectResponse>>> GetAll(
        [FromQuery] GetProjectsRequest request,
        CancellationToken cancellationToken)
    {
        var projects = await projectService.GetAllAsync(request, cancellationToken);

        return Ok(projects);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<ProjectDetailsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectDetailsResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var project = await projectService.GetByIdAsync(id, cancellationToken);

        return Ok(project);
    }

    [HttpPost]
    [ProducesResponseType<ProjectResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectResponse>> Create(
        [FromBody] CreateProjectRequest request,
        CancellationToken cancellationToken)
    {
        var project = await projectService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = project.Id },
            project);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<ProjectResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectResponse>> Update(
        Guid id,
        [FromBody] UpdateProjectRequest request,
        CancellationToken cancellationToken)
    {
        var project = await projectService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return Ok(project);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await projectService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
}