using AutoMapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using ProjectManagement.API.Contracts.Employees.Requests;
using ProjectManagement.API.Exceptions;
using ProjectManagement.API.Services.Employees;
using ProjectManagement.Tests.Helpers;

namespace ProjectManagement.Tests.Services;

public class EmployeeServiceTests
{
    private readonly IMapper _mapper = MapperFactory.Create();

    [Fact]
    public async Task CreateAsync_ShouldCreateEmployee()
    {
        // Arrange
        await using var context = DbContextFactory.Create(out SqliteConnection connection);
        await using var _ = connection;

        var service = new EmployeeService(context, _mapper);

        var request = new CreateEmployeeRequest
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            MiddleName = "Ivanovich",
            Email = "ivan@test.com"
        };

        // Act
        var response = await service.CreateAsync(request);

        // Assert
        response.Should().NotBeNull();
        response.FirstName.Should().Be("Ivan");
        response.LastName.Should().Be("Ivanov");
        response.MiddleName.Should().Be("Ivanovich");
        response.Email.Should().Be("ivan@test.com");

        context.Employees.Should().ContainSingle();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowConflictException_WhenEmailAlreadyExists()
    {
        // Arrange
        await using var context = DbContextFactory.Create(out SqliteConnection connection);
        await using var _ = connection;

        var service = new EmployeeService(context, _mapper);

        await service.CreateAsync(new CreateEmployeeRequest
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan@test.com"
        });

        // Act
        var act = async () =>
            await service.CreateAsync(new CreateEmployeeRequest
            {
                FirstName = "Petr",
                LastName = "Petrov",
                Email = "ivan@test.com"
            });

        // Assert
        await act.Should().ThrowAsync<ConflictException>();
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnEmployee_WhenEmployeeExists()
    {
        // Arrange
        await using var context = DbContextFactory.Create(out SqliteConnection connection);
        await using var _ = connection;

        var service = new EmployeeService(context, _mapper);

        var created = await service.CreateAsync(new CreateEmployeeRequest
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan@test.com"
        });

        // Act
        var employee = await service.GetByIdAsync(created.Id);

        // Assert
        employee.Id.Should().Be(created.Id);
        employee.Email.Should().Be("ivan@test.com");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenEmployeeDoesNotExist()
    {
        // Arrange
        await using var context = DbContextFactory.Create(out SqliteConnection connection);
        await using var _ = connection;

        var service = new EmployeeService(context, _mapper);

        // Act
        var act = async () => await service.GetByIdAsync(Guid.CreateVersion7());

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmployeesOrderedByLastName()
    {
        // Arrange
        await using var context = DbContextFactory.Create(out SqliteConnection connection);
        await using var _ = connection;

        var service = new EmployeeService(context, _mapper);

        await service.CreateAsync(new()
        {
            FirstName = "Ivan",
            LastName = "Zaycev",
            Email = "1@test.com"
        });

        await service.CreateAsync(new()
        {
            FirstName = "Petr",
            LastName = "Antonov",
            Email = "2@test.com"
        });

        // Act
        var employees = await service.GetAllAsync();

        // Assert
        employees.Should().HaveCount(2);

        employees.First().LastName.Should().Be("Antonov");
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdateEmployee()
    {
        // Arrange
        await using var context = DbContextFactory.Create(out SqliteConnection connection);
        await using var _ = connection;

        var service = new EmployeeService(context, _mapper);

        var employee = await service.CreateAsync(new()
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan@test.com"
        });

        // Act
        var updated = await service.UpdateAsync(
            employee.Id,
            new UpdateEmployeeRequest
            {
                FirstName = "Petr",
                LastName = "Petrov",
                Email = "petr@test.com"
            });

        // Assert
        updated.FirstName.Should().Be("Petr");
        updated.LastName.Should().Be("Petrov");
        updated.Email.Should().Be("petr@test.com");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFoundException_WhenEmployeeDoesNotExist()
    {
        await using var context = DbContextFactory.Create(out SqliteConnection connection);
        await using var _ = connection;

        var service = new EmployeeService(context, _mapper);

        var act = async () =>
            await service.UpdateAsync(
                Guid.CreateVersion7(),
                new UpdateEmployeeRequest
                {
                    FirstName = "Ivan",
                    LastName = "Ivanov",
                    Email = "ivan@test.com"
                });

        await act.Should().ThrowAsync<NotFoundException>();
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldThrowConflictException_WhenEmailAlreadyExists()
    {
        // Arrange
        await using var context = DbContextFactory.Create(out var connection);
        await using var _ = connection;

        var service = new EmployeeService(context, _mapper);

        var employee1 = await service.CreateAsync(new CreateEmployeeRequest
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan@test.com"
        });

        var employee2 = await service.CreateAsync(new CreateEmployeeRequest
        {
            FirstName = "Petr",
            LastName = "Petrov",
            Email = "petr@test.com"
        });

        // Act
        var act = () => service.UpdateAsync(employee2.Id, new UpdateEmployeeRequest
        {
            FirstName = "Petr",
            LastName = "Petrov",
            Email = "ivan@test.com"
        });

        // Assert
        await act.Should().ThrowAsync<ConflictException>();
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldRemoveEmployee()
    {
        await using var context = DbContextFactory.Create(out SqliteConnection connection);
        await using var _ = connection;

        var service = new EmployeeService(context, _mapper);

        var employee = await service.CreateAsync(new()
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan@test.com"
        });

        await service.DeleteAsync(employee.Id);

        context.Employees.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowNotFoundException_WhenEmployeeDoesNotExist()
    {
        await using var context = DbContextFactory.Create(out SqliteConnection connection);
        await using var _ = connection;

        var service = new EmployeeService(context, _mapper);

        var act = async () => await service.DeleteAsync(Guid.CreateVersion7());

        await act.Should().ThrowAsync<NotFoundException>();
    }
    
    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingEmployees()
    {
        await using var context = DbContextFactory.Create(out SqliteConnection connection);
        await using var _ = connection;

        var service = new EmployeeService(context, _mapper);

        await service.CreateAsync(new()
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan@test.com"
        });

        await service.CreateAsync(new()
        {
            FirstName = "Petr",
            LastName = "Petrov",
            Email = "petr@test.com"
        });

        var result = await service.SearchAsync("Ivan");

        result.Should().ContainSingle();
        result.Single().FirstName.Should().Be("Ivan");
    }
    
    [Fact]
    public async Task SearchAsync_ShouldReturnAllEmployeesWithSameFirstName()
    {
        // Arrange
        await using var context = DbContextFactory.Create(out var connection);
        await using var _ = connection;

        var service = new EmployeeService(context, _mapper);

        await service.CreateAsync(new CreateEmployeeRequest
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "ivan1@test.com"
        });

        await service.CreateAsync(new CreateEmployeeRequest
        {
            FirstName = "Ivan",
            LastName = "Petrov",
            Email = "ivan2@test.com"
        });

        await service.CreateAsync(new CreateEmployeeRequest
        {
            FirstName = "Petr",
            LastName = "Sidorov",
            Email = "petr@test.com"
        });

        // Act
        var result = await service.SearchAsync("Ivan");

        // Assert
        result.Should().HaveCount(2);

        result.Should().OnlyContain(e => e.FirstName == "Ivan");
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnEmptyCollection_WhenNothingFound()
    {
        await using var context = DbContextFactory.Create(out SqliteConnection connection);
        await using var _ = connection;

        var service = new EmployeeService(context, _mapper);

        var result = await service.SearchAsync("Unknown");

        result.Should().BeEmpty();
    }
}