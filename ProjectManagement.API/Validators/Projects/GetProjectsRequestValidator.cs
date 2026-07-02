using FluentValidation;
using ProjectManagement.API.Contracts.Projects.Requests;

namespace ProjectManagement.API.Validators.Projects;

public class GetProjectsRequestValidator : AbstractValidator<GetProjectsRequest>
{
    public GetProjectsRequestValidator()
    {
        RuleFor(request => request.Priority)
            .GreaterThan(0)
            .When(request => request.Priority.HasValue);

        RuleFor(request => request)
            .Must(request =>
                request.StartDateFrom is null ||
                request.StartDateTo is null ||
                request.StartDateTo >= request.StartDateFrom)
            .WithMessage("End date must be before start date.");
    }
}