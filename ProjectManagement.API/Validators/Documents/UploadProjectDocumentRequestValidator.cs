using FluentValidation;
using ProjectManagement.API.Contracts.ProjectDocuments.Requests;

namespace ProjectManagement.API.Validators.Documents;

public class UploadProjectDocumentRequestValidator : AbstractValidator<UploadProjectDocumentRequest>
{
    private const long MaxFileSize = 10 * 1024 * 1024;

    private static readonly string[] AllowedExtensions =
    [
        ".pdf",
        ".doc",
        ".docx",
        ".xls",
        ".xlsx"
    ];

    public UploadProjectDocumentRequestValidator()
    {
        RuleFor(request => request.File)
            .NotNull();

        RuleFor(request => request.File.Length)
            .LessThanOrEqualTo(MaxFileSize);

        RuleFor(request => Path.GetExtension(request.File.FileName))
            .Must(AllowedExtensions.Contains)
            .WithMessage("Invalid file extension.");
    }
}