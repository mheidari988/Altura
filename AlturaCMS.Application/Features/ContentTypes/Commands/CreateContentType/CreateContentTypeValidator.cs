using FluentValidation;

namespace AlturaCMS.Application.Features.ContentTypes.Commands.CreateContentType;

public class CreateContentTypeValidator : AbstractValidator<CreateContentTypeCommand>
{
    public CreateContentTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Fields)
            .NotEmpty().WithMessage("Fields are required.")
            .ForEach(field => field.SetValidator(new CreateContentTypeFieldValidator()));
    }
}

public class CreateContentTypeFieldValidator : AbstractValidator<CreateContentTypeFieldDto>
{
    public CreateContentTypeFieldValidator()
    {
        RuleFor(x => x.FieldId)
            .NotNull().WithMessage("FieldId is required.");
    }
}
