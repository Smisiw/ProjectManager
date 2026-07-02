using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.API.Contracts.Projects.Enums;
using ProjectManagement.API.Contracts.Projects.Requests;
using ProjectManagement.API.Contracts.Projects.Responses;
using ProjectManagement.API.Data;
using ProjectManagement.API.Domain.Entities;
using ProjectManagement.API.Exceptions;

namespace ProjectManagement.API.Services.Projects;

public class ProjectService(
    AppDbContext context,
    IMapper mapper) : IProjectService
{
    public async Task<IReadOnlyCollection<ProjectResponse>> GetAllAsync(
        GetProjectsRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = context.Projects.AsNoTracking();
        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(p => p.Name.Contains(request.Search) ||
                                     p.CustomerCompany.Contains(request.Search) ||
                                     p.ExecutorCompany.Contains(request.Search));
        }

        if (request.Priority is not null)
        {
            query = query.Where(p => p.Priority == request.Priority);
        }

        if (request.ManagerId is not null)
        {
            query = query.Where(p => p.ManagerId == request.ManagerId);
        }

        if (request.StartDateFrom is not null)
        {
            query = query.Where(p => p.StartDate >= request.StartDateFrom);
        }

        if (request.StartDateTo is not null)
        {
            query = query.Where(p => p.StartDate <= request.StartDateTo);
        }
        
        query =  ApplySorting(query, request);
        
        return await query
            .ProjectTo<ProjectResponse>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProjectDetailsResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var project = await context.Projects
            .AsNoTracking()
            .ProjectTo<ProjectDetailsResponse>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        return project
            ?? throw new NotFoundException(nameof(Project), id);
    }

    public async Task<ProjectDetailsResponse> CreateAsync(
        CreateProjectRequest request,
        CancellationToken cancellationToken = default)
    {
        var manager = await GetManagerAsync(request.ManagerId, cancellationToken);

        var employees = await GetEmployeesAsync(
            request.EmployeeIds,
            cancellationToken);
        
        if (employees.All(e => e.Id != manager.Id))
        {
            employees.Add(manager);
        }

        var project = mapper.Map<Project>(request);

        project.Manager = manager;

        foreach (var employee in employees)
        {
            project.Employees.Add(employee);
        }

        await context.Projects.AddAsync(project, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<ProjectDetailsResponse>(project);
    }

    public async Task<ProjectDetailsResponse> UpdateAsync(
        Guid id,
        UpdateProjectRequest request,
        CancellationToken cancellationToken = default)
    {
        var project = await context.Projects
                          .Include(p => p.Employees)
                          .Include(p => p.Manager)
                          .FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
                      ?? throw new NotFoundException(nameof(Project), id);

        var manager = await GetManagerAsync(request.ManagerId, cancellationToken);

        var employees = await GetEmployeesAsync(
            request.EmployeeIds,
            cancellationToken);
        
        if (employees.All(e => e.Id != manager.Id))
        {
            employees.Add(manager);
        }

        mapper.Map(request, project);

        project.Manager = manager;

        project.Employees.Clear();

        foreach (var employee in employees)
        {
            project.Employees.Add(employee);
        }

        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<ProjectDetailsResponse>(project);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var project = await context.Projects
            .FindAsync([id], cancellationToken);
        if (project is null)
            throw new NotFoundException(nameof(Project), id);
        context.Projects.Remove(project);
        await context.SaveChangesAsync(cancellationToken);
    }
    
    private async Task<Employee> GetManagerAsync(
        Guid managerId,
        CancellationToken cancellationToken)
    {
        return await context.Employees
                   .FirstOrDefaultAsync(e => e.Id == managerId, cancellationToken)
               ?? throw new NotFoundException(nameof(Employee), managerId);
    }
    
    private async Task<List<Employee>> GetEmployeesAsync(
        IReadOnlyCollection<Guid> employeeIds,
        CancellationToken cancellationToken)
    {
        if (employeeIds.Count == 0)
        {
            return [];
        }

        var employees = await context.Employees
            .Where(e => employeeIds.Contains(e.Id))
            .ToListAsync(cancellationToken);

        if (employees.Count != employeeIds.Count)
        {
            throw new NotFoundException("One or more employees were not found.");
        }

        return employees;
    }
    
    private static IQueryable<Project> ApplySorting(
        IQueryable<Project> query,
        GetProjectsRequest request)
    {
        return (request.SortBy, request.Descending) switch
        {
            (ProjectSortField.Name, false) => query.OrderBy(p => p.Name),
            (ProjectSortField.Name, true) => query.OrderByDescending(p => p.Name),

            (ProjectSortField.Priority, false) => query.OrderBy(p => p.Priority),
            (ProjectSortField.Priority, true) => query.OrderByDescending(p => p.Priority),

            (ProjectSortField.StartDate, false) => query.OrderBy(p => p.StartDate),
            (ProjectSortField.StartDate, true) => query.OrderByDescending(p => p.StartDate),

            (ProjectSortField.EndDate, false) => query.OrderBy(p => p.EndDate),
            (ProjectSortField.EndDate, true) => query.OrderByDescending(p => p.EndDate),

            (ProjectSortField.CustomerCompany, false) => query.OrderBy(p => p.CustomerCompany),
            (ProjectSortField.CustomerCompany, true) => query.OrderByDescending(p => p.CustomerCompany),

            (ProjectSortField.ExecutorCompany, false) => query.OrderBy(p => p.ExecutorCompany),
            (ProjectSortField.ExecutorCompany, true) => query.OrderByDescending(p => p.ExecutorCompany),

            _ => query.OrderBy(p => p.Name)
        };
    }
}