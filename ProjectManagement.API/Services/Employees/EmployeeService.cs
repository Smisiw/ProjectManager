using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.API.Contracts.Employees.Requests;
using ProjectManagement.API.Contracts.Employees.Responses;
using ProjectManagement.API.Data;
using ProjectManagement.API.Domain.Entities;
using ProjectManagement.API.Exceptions;

namespace ProjectManagement.API.Services.Employees;

public class EmployeeService(
    AppDbContext context,
    IMapper mapper)
    : IEmployeeService
{
    public async Task<IReadOnlyCollection<EmployeeResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Employees
            .AsNoTracking()
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ProjectTo<EmployeeResponse>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<EmployeeResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var employee = await context.Employees
                           .AsNoTracking()
                           .Where(e => e.Id == id)
                           .ProjectTo<EmployeeResponse>(mapper.ConfigurationProvider)
                           .FirstOrDefaultAsync(cancellationToken)
                       ?? throw new NotFoundException(nameof(Employee), id);

        return employee;
    }

    public async Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken = default)
    {
        var exists = await context.Employees
            .AnyAsync(
                e => e.Email == request.Email,
                cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                nameof(Employee),
                nameof(request.Email),
                request.Email);
        }
        
        var employee = mapper.Map<Employee>(request);

        context.Employees.Add(employee);

        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<EmployeeResponse>(employee);
    }

    public async Task<EmployeeResponse> UpdateAsync(Guid id, UpdateEmployeeRequest request, CancellationToken cancellationToken = default)
    {
        var exists = await context.Employees
            .AnyAsync(
                e => e.Email == request.Email &&
                e.Id != id,
                cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                nameof(Employee),
                nameof(request.Email),
                request.Email);
        }
        
        var employee = await context.Employees
                           .FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
                       ?? throw new NotFoundException(nameof(Employee), id);

        mapper.Map(request, employee);

        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<EmployeeResponse>(employee);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var employee = await context.Employees
                           .FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
                       ?? throw new NotFoundException(nameof(Employee), id);

        context.Employees.Remove(employee);

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<EmployeeResponse>> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        query = query.Trim();

        return await context.Employees
            .AsNoTracking()
            .Where(e =>
                e.FirstName.Contains(query) ||
                e.LastName.Contains(query) ||
                (e.MiddleName != null && e.MiddleName.Contains(query)) ||
                e.Email.Contains(query))
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ProjectTo<EmployeeResponse>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}