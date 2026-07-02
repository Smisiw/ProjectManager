using ProjectManagement.API.Contracts.Employees.Responses;
using ProjectManagement.API.Contracts.ProjectDocuments.Responses;

namespace ProjectManagement.API.Contracts.Projects.Responses;

public sealed record ProjectDetailsResponse
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string CustomerCompany { get; init; }

    public required string ExecutorCompany { get; init; }

    public required DateOnly StartDate { get; init; }

    public required DateOnly EndDate { get; init; }

    public required int Priority { get; init; }

    public required EmployeeResponse Manager { get; init; }

    public IReadOnlyCollection<EmployeeResponse> Employees { get; init; } = [];

    public IReadOnlyCollection<ProjectDocumentResponse> Documents { get; init; } = [];
}