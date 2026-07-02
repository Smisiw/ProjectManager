using FluentValidation.TestHelper;
using ProjectManagement.API.Contracts.Employees.Requests;
using ProjectManagement.API.Validators.Employees;

namespace ProjectManagement.Tests.Validators;

public sealed class CreateEmployeeRequestValidatorTests
{
    private readonly CreateEmployeeRequestValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_FirstName_Is_Empty()
    {
        var model = new CreateEmployeeRequest
        {
            FirstName = string.Empty,
            LastName = "Ivanov",
            Email = "ivan@test.com"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Fact]
    public void Should_Have_Error_When_LastName_Is_Empty()
    {
        var model = new CreateEmployeeRequest
        {
            FirstName = "Ivan",
            LastName = string.Empty,
            Email = "ivan@test.com"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new CreateEmployeeRequest
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "invalid-email"
        };

        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Not_Have_Validation_Errors()
    {
        var model = new CreateEmployeeRequest
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            MiddleName = "Ivanovich",
            Email = "ivan@test.com"
        };

        var result = _validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }
}