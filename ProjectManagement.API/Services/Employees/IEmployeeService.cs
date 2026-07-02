using ProjectManagement.API.Contracts.Employees.Requests;
using ProjectManagement.API.Contracts.Employees.Responses;

namespace ProjectManagement.API.Services.Employees;

public interface IEmployeeService
{
    Task<IReadOnlyCollection<EmployeeResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<EmployeeResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken = default);

    Task<EmployeeResponse> UpdateAsync(Guid id, UpdateEmployeeRequest request, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<EmployeeResponse>> SearchAsync(string query, CancellationToken cancellationToken = default);
}