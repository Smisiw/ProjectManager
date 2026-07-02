using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.API.Contracts.Employees.Requests;
using ProjectManagement.API.Contracts.Employees.Responses;
using ProjectManagement.API.Contracts.Projects.Enums;
using ProjectManagement.API.Contracts.Projects.Requests;
using ProjectManagement.API.Exceptions;
using ProjectManagement.API.Services.Employees;
using ProjectManagement.API.Services.Projects;
using ProjectManagement.Tests.Helpers;

namespace ProjectManagement.Tests.Services;

public class ProjectServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateProject()
    {
        // Arrange
        await using var context = DbContextFactory.Create(out var connection);
        await using var _ = connection;

        var mapper = MapperFactory.Create();

        var employeeService = new EmployeeService(context, mapper);
        var projectService = new ProjectService(context, mapper);

        var manager = await employeeService.CreateAsync(new CreateEmployeeRequest
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            MiddleName = "Ivanovich",
            Email = "ivan.ivanov@test.com"
        });

        var employee = await employeeService.CreateAsync(new CreateEmployeeRequest
        {
            FirstName = "Petr",
            LastName = "Petrov",
            MiddleName = "Petrovich",
            Email = "petr.petrov@test.com"
        });

        var request = new CreateProjectRequest
        {
            Name = "CRM System",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = manager.Id,
            EmployeeIds = [employee.Id]
        };

        // Act
        var response = await projectService.CreateAsync(request);

        // Assert
        response.Should().NotBeNull();
        response.Name.Should().Be("CRM System");

        var project = await context.Projects
            .Include(p => p.Manager)
            .Include(p => p.Employees)
            .SingleAsync();

        project.Name.Should().Be(request.Name);
        project.CustomerCompany.Should().Be(request.CustomerCompany);
        project.ExecutorCompany.Should().Be(request.ExecutorCompany);
        project.Priority.Should().Be(5);

        project.Manager.Id.Should().Be(manager.Id);

        project.Employees.Should().Contain(e => e.Id == manager.Id);
        project.Employees.Should().Contain(e => e.Id == employee.Id);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldThrowNotFoundException_WhenManagerDoesNotExist()
    {
        await using var context = DbContextFactory.Create(out var connection);
        await using var _ = connection;

        var mapper = MapperFactory.Create();

        var employeeService = new EmployeeService(context, mapper);
        var projectService = new ProjectService(context, mapper);

        var employee = await employeeService.CreateAsync(new CreateEmployeeRequest
        {
            FirstName = "Petr",
            LastName = "Petrov",
            Email = "petr.petrov@test.com"
        });

        var request = new CreateProjectRequest
        {
            Name = "CRM System",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = Guid.CreateVersion7(),
            EmployeeIds = [employee.Id]
        };

        var act = () => projectService.CreateAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }
    
    [Fact]
    public async Task CreateAsync_ShouldThrowNotFoundException_WhenEmployeeDoesNotExist()
    {
        await using var context = DbContextFactory.Create(out var connection);
        await using var _ = connection;

        var mapper = MapperFactory.Create();

        var employeeService = new EmployeeService(context, mapper);
        var projectService = new ProjectService(context, mapper);

        var manager = await employeeService.CreateAsync(new CreateEmployeeRequest
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan.ivanov@test.com"
        });

        var request = new CreateProjectRequest
        {
            Name = "CRM System",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = manager.Id,
            EmployeeIds = [Guid.CreateVersion7()]
        };

        var act = () => projectService.CreateAsync(request);

        await act.Should().ThrowAsync<NotFoundException>();
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnProject()
    {
        await using var context = DbContextFactory.Create(out var connection);
        await using var _ = connection;

        var mapper = MapperFactory.Create();

        var employeeService = new EmployeeService(context, mapper);
        var projectService = new ProjectService(context, mapper);

        var manager = await employeeService.CreateAsync(new CreateEmployeeRequest
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan.ivanov@test.com"
        });

        var created = await projectService.CreateAsync(new CreateProjectRequest
        {
            Name = "CRM System",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = manager.Id,
            EmployeeIds = []
        });

        // Act
        var project = await projectService.GetByIdAsync(created.Id);

        // Assert
        project.Id.Should().Be(created.Id);
        project.Name.Should().Be("CRM System");
        project.Manager.Id.Should().Be(manager.Id);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdateProject()
    {
        await using var context = DbContextFactory.Create(out var connection);
        await using var _ = connection;

        var mapper = MapperFactory.Create();

        var employeeService = new EmployeeService(context, mapper);
        var projectService = new ProjectService(context, mapper);

        var manager = await employeeService.CreateAsync(new()
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan@test.com"
        });

        var project = await projectService.CreateAsync(new CreateProjectRequest
        {
            Name = "CRM System",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = manager.Id,
            EmployeeIds = []
        });

        var updated = await projectService.UpdateAsync(project.Id, new UpdateProjectRequest
        {
            Name = "ERP System",
            CustomerCompany = "Google",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 2, 1),
            EndDate = new(2025, 11, 30),
            Priority = 3,
            ManagerId = manager.Id,
            EmployeeIds = []
        });

        updated.Name.Should().Be("ERP System");
        updated.CustomerCompany.Should().Be("Google");
        updated.Priority.Should().Be(3);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldDeleteProject()
    {
        await using var context = DbContextFactory.Create(out var connection);
        await using var _ = connection;

        var mapper = MapperFactory.Create();

        var employeeService = new EmployeeService(context, mapper);
        var projectService = new ProjectService(context, mapper);

        var manager = await employeeService.CreateAsync(new()
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan@test.com"
        });

        var project = await projectService.CreateAsync(new CreateProjectRequest
        {
            Name = "CRM System",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = manager.Id,
            EmployeeIds = []
        });

        await projectService.DeleteAsync(project.Id);

        context.Projects.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldFilterByPriority()
    {
        // Arrange
        await using var context = DbContextFactory.Create(out var connection);
        await using var _ = connection;

        var mapper = MapperFactory.Create();

        var employeeService = new EmployeeService(context, mapper);
        var projectService = new ProjectService(context, mapper);

        var manager = await employeeService.CreateAsync(new()
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan@test.com"
        });

        await projectService.CreateAsync(new ()
        {
            Name = "CRM System",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = manager.Id
        });

        await projectService.CreateAsync(new()
        {
            Name = "ERP System",
            CustomerCompany = "Google",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 2, 1),
            EndDate = new(2025, 12, 31),
            Priority = 3,
            ManagerId = manager.Id
        });

        // Act
        var result = await projectService.GetAllAsync(new GetProjectsRequest
        {
            Priority = 5
        });

        // Assert
        result.Should().HaveCount(1);
        result.Single().Priority.Should().Be(5);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldFilterByManager()
    {
        await using var context = DbContextFactory.Create(out var connection);
        await using var _ = connection;

        var mapper = MapperFactory.Create();

        var employeeService = new EmployeeService(context, mapper);
        var projectService = new ProjectService(context, mapper);

        var manager1 = await employeeService.CreateAsync(new()
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan@test.com"
        });

        var manager2 = await employeeService.CreateAsync(new()
        {
            FirstName = "Petr",
            LastName = "Petrov",
            Email = "petr@test.com"
        });

        await projectService.CreateAsync(new()
        {
            Name = "CRM System",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = manager1.Id
        });

        await projectService.CreateAsync(new()
        {
            Name = "ERP System",
            CustomerCompany = "Google",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 2, 1),
            EndDate = new(2025, 12, 31),
            Priority = 3,
            ManagerId = manager2.Id
        });

        var result = await projectService.GetAllAsync(new()
        {
            ManagerId = manager2.Id
        });

        result.Should().ContainSingle();
        result.Single().Manager.Id.Should().Be(manager2.Id);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldFilterByStartDateFrom()
    {
        // Arrange
        await using var context = DbContextFactory.Create(out var connection);
        await using var _ = connection;

        var mapper = MapperFactory.Create();

        var employeeService = new EmployeeService(context, mapper);
        var projectService = new ProjectService(context, mapper);

        var manager = await employeeService.CreateAsync(new()
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan@test.com"
        });

        await projectService.CreateAsync(new()
        {
            Name = "CRM System",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = manager.Id
        });

        await projectService.CreateAsync(new()
        {
            Name = "ERP System",
            CustomerCompany = "Google",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 6, 1),
            EndDate = new(2025, 12, 31),
            Priority = 3,
            ManagerId = manager.Id
        });

        // Act
        var result = await projectService.GetAllAsync(new()
        {
            StartDateFrom = new DateOnly(2025, 5, 1)
        });

        // Assert
        result.Should().ContainSingle();
        result.Single().Name.Should().Be("ERP System");
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldFilterByStartDateTo()
    {
        // Arrange
        await using var context = DbContextFactory.Create(out var connection);
        await using var _ = connection;

        var mapper = MapperFactory.Create();

        var employeeService = new EmployeeService(context, mapper);
        var projectService = new ProjectService(context, mapper);

        var manager = await employeeService.CreateAsync(new()
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan@test.com"
        });

        await projectService.CreateAsync(new()
        {
            Name = "CRM System",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = manager.Id
        });

        await projectService.CreateAsync(new()
        {
            Name = "ERP System",
            CustomerCompany = "Google",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 6, 1),
            EndDate = new(2025, 12, 31),
            Priority = 3,
            ManagerId = manager.Id
        });

        // Act
        var result = await projectService.GetAllAsync(new()
        {
            StartDateTo = new DateOnly(2025, 3, 1)
        });

        // Assert
        result.Should().ContainSingle();
        result.Single().Name.Should().Be("CRM System");
    }
    
    [Fact]
public async Task GetAllAsync_ShouldFilterByStartDateRange()
{
    // Arrange
    await using var context = DbContextFactory.Create(out var connection);
    await using var _ = connection;

    var mapper = MapperFactory.Create();

    var employeeService = new EmployeeService(context, mapper);
    var projectService = new ProjectService(context, mapper);

    var manager = await employeeService.CreateAsync(new CreateEmployeeRequest
    {
        FirstName = "Ivan",
        LastName = "Ivanov",
        MiddleName = "Ivanovich",
        Email = "ivan.ivanov@test.com"
    });

    await projectService.CreateAsync(new CreateProjectRequest
    {
        Name = "CRM System",
        CustomerCompany = "Microsoft",
        ExecutorCompany = "Amazon",
        StartDate = new DateOnly(2025, 1, 1),
        EndDate = new DateOnly(2025, 12, 31),
        Priority = 5,
        ManagerId = manager.Id
    });

    await projectService.CreateAsync(new CreateProjectRequest
    {
        Name = "ERP System",
        CustomerCompany = "Google",
        ExecutorCompany = "Amazon",
        StartDate = new DateOnly(2025, 4, 1),
        EndDate = new DateOnly(2025, 12, 31),
        Priority = 3,
        ManagerId = manager.Id
    });

    await projectService.CreateAsync(new CreateProjectRequest
    {
        Name = "HR Portal",
        CustomerCompany = "Netflix",
        ExecutorCompany = "Amazon",
        StartDate = new DateOnly(2025, 8, 1),
        EndDate = new DateOnly(2025, 12, 31),
        Priority = 1,
        ManagerId = manager.Id
    });

    // Act
    var result = await projectService.GetAllAsync(new GetProjectsRequest
    {
        StartDateFrom = new DateOnly(2025, 3, 1),
        StartDateTo = new DateOnly(2025, 6, 1)
    });

    // Assert
    result.Should().ContainSingle();

    var project = result.Single();

    project.Name.Should().Be("ERP System");
    project.CustomerCompany.Should().Be("Google");
    project.StartDate.Should().Be(new DateOnly(2025, 4, 1));
}
    
    [Fact]
    public async Task GetAllAsync_ShouldFilterBySearch()
    {
        await using var context = DbContextFactory.Create(out var connection);
        await using var _ = connection;

        var mapper = MapperFactory.Create();

        var employeeService = new EmployeeService(context, mapper);
        var projectService = new ProjectService(context, mapper);

        var manager = await employeeService.CreateAsync(new()
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan@test.com"
        });

        await projectService.CreateAsync(new()
        {
            Name = "CRM System",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = manager.Id
        });

        await projectService.CreateAsync(new()
        {
            Name = "HR Portal",
            CustomerCompany = "Google",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 2, 1),
            EndDate = new(2025, 12, 31),
            Priority = 2,
            ManagerId = manager.Id
        });

        var result = await projectService.GetAllAsync(new()
        {
            Search = "CRM"
        });

        result.Should().ContainSingle();
        result.Single().Name.Should().Be("CRM System");
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldSortByName()
    {
        await using var context = DbContextFactory.Create(out var connection);
        await using var _ = connection;

        var mapper = MapperFactory.Create();

        var employeeService = new EmployeeService(context, mapper);
        var projectService = new ProjectService(context, mapper);

        var manager = await employeeService.CreateAsync(new()
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan@test.com"
        });

        await projectService.CreateAsync(new()
        {
            Name = "Zoo",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = manager.Id
        });

        await projectService.CreateAsync(new()
        {
            Name = "Alpha",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 2, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = manager.Id
        });

        var result = await projectService.GetAllAsync(new()
        {
            SortBy = ProjectSortField.Name
        });

        result.First().Name.Should().Be("Alpha");
        result.Last().Name.Should().Be("Zoo");
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldSortByPriorityDescending()
    {
        await using var context = DbContextFactory.Create(out var connection);
        await using var _ = connection;

        var mapper = MapperFactory.Create();

        var employeeService = new EmployeeService(context, mapper);
        var projectService = new ProjectService(context, mapper);

        var manager = await employeeService.CreateAsync(new()
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan@test.com"
        });

        await projectService.CreateAsync(new()
        {
            Name = "Low",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 1,
            ManagerId = manager.Id
        });

        await projectService.CreateAsync(new()
        {
            Name = "High",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 2, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = manager.Id
        });

        var result = await projectService.GetAllAsync(new()
        {
            SortBy = ProjectSortField.Priority,
            Descending = true
        });

        result.First().Priority.Should().Be(5);
        result.Last().Priority.Should().Be(1);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReplaceEmployees()
    {
        // Arrange
        await using var context = DbContextFactory.Create(out var connection);
        await using var _ = connection;

        var mapper = MapperFactory.Create();

        var employeeService = new EmployeeService(context, mapper);
        var projectService = new ProjectService(context, mapper);

        var manager = await employeeService.CreateAsync(new()
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan@test.com"
        });

        var employee1 = await employeeService.CreateAsync(new()
        {
            FirstName = "Petr",
            LastName = "Petrov",
            Email = "petr@test.com"
        });

        var employee2 = await employeeService.CreateAsync(new()
        {
            FirstName = "Sergey",
            LastName = "Sergeev",
            Email = "sergey@test.com"
        });

        var project = await projectService.CreateAsync(new()
        {
            Name = "CRM System",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = manager.Id,
            EmployeeIds = [employee1.Id]
        });

        // Act
        await projectService.UpdateAsync(project.Id, new()
        {
            Name = project.Name,
            CustomerCompany = project.CustomerCompany,
            ExecutorCompany = project.ExecutorCompany,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Priority = project.Priority,
            ManagerId = manager.Id,
            EmployeeIds = [employee2.Id]
        });

        // Assert
        var updated = await context.Projects
            .Include(p => p.Employees)
            .SingleAsync();

        updated.Employees.Should().Contain(e => e.Id == manager.Id);
        updated.Employees.Should().Contain(e => e.Id == employee2.Id);
        updated.Employees.Should().NotContain(e => e.Id == employee1.Id);
    }
    
    private async Task<EmployeeResponse> CreateEmployeeAsync(
        EmployeeService employeeService,
        string firstName,
        string lastName,
        string email)
    {
        return await employeeService.CreateAsync(new CreateEmployeeRequest
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email
        });
    }
}