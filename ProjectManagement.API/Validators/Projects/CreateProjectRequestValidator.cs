using FluentValidation;
using ProjectManagement.API.Contracts.Projects.Requests;

namespace ProjectManagement.API.Validators.Projects;

public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(255);
        RuleFor(request => request.CustomerCompany)
            .NotEmpty()
            .MaximumLength(255);
        RuleFor(request => request.ExecutorCompany)
            .NotEmpty()
            .MaximumLength(255);
        RuleFor(request => request.Priority)
            .NotEmpty()
            .GreaterThan(0);
        RuleFor(request => request.ManagerId)
            .NotEmpty();
        RuleFor(request => request.StartDate)
            .NotEmpty();
        RuleFor(request => request.EndDate)
            .NotEmpty();
        RuleFor(request => request)
            .Must(request => request.EndDate >= request.StartDate)
            .WithMessage("End date must be before start date.");
    }
}