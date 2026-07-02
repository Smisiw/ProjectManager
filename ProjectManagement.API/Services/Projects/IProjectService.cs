using ProjectManagement.API.Contracts.Projects.Requests;
using ProjectManagement.API.Contracts.Projects.Responses;

namespace ProjectManagement.API.Services.Projects;

public interface IProjectService
{
    Task<IReadOnlyCollection<ProjectResponse>> GetAllAsync(
        GetProjectsRequest request,
        CancellationToken cancellationToken = default);

    Task<ProjectDetailsResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<ProjectDetailsResponse> CreateAsync(
        CreateProjectRequest request,
        CancellationToken cancellationToken = default);

    Task<ProjectDetailsResponse> UpdateAsync(
        Guid id,
        UpdateProjectRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}