using AutoMapper;
using ProjectManagement.API.Contracts.Projects.Requests;
using ProjectManagement.API.Contracts.Projects.Responses;
using ProjectManagement.API.Domain.Entities;

namespace ProjectManagement.API.Mappings;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<CreateProjectRequest, Project>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Manager, o => o.Ignore())
            .ForMember(d => d.Employees, o => o.Ignore())
            .ForMember(d => d.Documents, o => o.Ignore());
        CreateMap<UpdateProjectRequest, Project>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Manager, o => o.Ignore())
            .ForMember(d => d.Employees, o => o.Ignore())
            .ForMember(d => d.Documents, o => o.Ignore());
        CreateMap<Project, ProjectResponse>();
        CreateMap<Project, ProjectDetailsResponse>();
    }
}