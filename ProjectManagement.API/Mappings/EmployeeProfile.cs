using AutoMapper;
using ProjectManagement.API.Contracts.Employees.Requests;
using ProjectManagement.API.Contracts.Employees.Responses;
using ProjectManagement.API.Domain.Entities;

namespace ProjectManagement.API.Mappings;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<CreateEmployeeRequest, Employee>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.ManagedProjects, o => o.Ignore())
            .ForMember(d => d.Projects, o => o.Ignore());
        CreateMap<UpdateEmployeeRequest, Employee>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.ManagedProjects, o => o.Ignore())
            .ForMember(d => d.Projects, o => o.Ignore());
        CreateMap<Employee, EmployeeResponse>();
        CreateMap<Employee, EmployeeShortResponse>()
            .ForMember(
                d => d.FullName,
                o => o.MapFrom(e => $"{e.LastName} {e.FirstName} {e.MiddleName}"));
    }
}