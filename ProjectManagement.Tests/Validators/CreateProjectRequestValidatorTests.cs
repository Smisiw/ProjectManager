using FluentValidation.TestHelper;
using ProjectManagement.API.Contracts.Projects.Requests;
using ProjectManagement.API.Validators.Projects;

namespace ProjectManagement.Tests.Validators;

public class CreateProjectRequestValidatorTests
{
    private readonly CreateProjectRequestValidator _validator = new();
    
    [Fact]
    public void ValidRequest_ShouldNotHaveValidationErrors()
    {
        var request = new CreateProjectRequest
        {
            Name = "CRM System",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = Guid.CreateVersion7()
        };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void ShouldHaveError_WhenNameIsEmpty()
    {
        var request = new CreateProjectRequest
        {
            Name = "",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = Guid.CreateVersion7()
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
    
    [Fact]
    public void ShouldHaveError_WhenNameTooLong()
    {
        var request = new CreateProjectRequest
        {
            Name = new string('A', 256),
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 5,
            ManagerId = Guid.CreateVersion7()
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
    
    [Fact]
    public void ShouldHaveError_WhenPriorityLessThanOne()
    {
        var request = new CreateProjectRequest
        {
            Name = "CRM",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate = new(2025, 12, 31),
            Priority = 0,
            ManagerId = Guid.CreateVersion7()
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Priority);
    }
    
    [Fact]
    public void ShouldHaveError_WhenEndDateBeforeStartDate()
    {
        var request = new CreateProjectRequest
        {
            Name = "CRM",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 5, 1),
            EndDate = new(2025, 1, 1),
            Priority = 5,
            ManagerId = Guid.CreateVersion7()
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x);
    }
    
    [Fact]
    public void ShouldHaveError_WhenManagerIdIsEmpty()
    {
        var request = new CreateProjectRequest
        {
            Name = "CRM",
            CustomerCompany = "Microsoft",
            ExecutorCompany = "Amazon",
            StartDate = new(2025, 1, 1),
            EndDate =  new(2025, 12, 31),
            Priority = 5,
            ManagerId = Guid.Empty
        };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.ManagerId);
    }
    
}