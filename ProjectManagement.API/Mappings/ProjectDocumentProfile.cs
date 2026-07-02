using AutoMapper;
using ProjectManagement.API.Contracts.ProjectDocuments.Responses;
using ProjectManagement.API.Contracts.Projects.Responses;
using ProjectManagement.API.Domain.Entities;

namespace ProjectManagement.API.Mappings;

public class ProjectDocumentProfile : Profile
{
    public ProjectDocumentProfile()
    {
        CreateMap<ProjectDocument, ProjectDocumentResponse>();
    }
}