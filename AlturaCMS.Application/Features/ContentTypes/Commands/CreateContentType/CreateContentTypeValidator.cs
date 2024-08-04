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
        RuleFor(x => x.Name)
            .Must((dto, name) => !ApplicationShared.GetDefaultFields().Any(f => f.Name == name))
            .WithMessage(dto => $"Name '{dto.Name}' is reserved.");

        RuleFor(x => x.DisplayName)
            .Must((dto, displayName) => !ApplicationShared.GetDefaultFields().Any(f => f.DisplayName == displayName))
            .WithMessage(dto => $"DisplayName '{dto.DisplayName}' is reserved.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.DisplayName)
            .MaximumLength(200).WithMessage("DisplayName must not exceed 200 characters.");

        RuleFor(x => x.FieldType)
            .IsInEnum().WithMessage("FieldType is required.");

        RuleFor(x => x.MinLength)
            .GreaterThanOrEqualTo(0).WithMessage("MinLength must be greater than or equal to 0.")
            .LessThanOrEqualTo(x => x.MaxLength ?? int.MaxValue).WithMessage("MinLength must be less than or equal to MaxLength.");

        RuleFor(x => x.MaxLength)
            .GreaterThanOrEqualTo(0).WithMessage("MaxLength must be greater than or equal to 0.")
            .GreaterThanOrEqualTo(x => x.MinLength ?? 0).WithMessage("MaxLength must be greater than or equal to MinLength.");

        RuleFor(x => x.MinValue)
            .LessThanOrEqualTo(x => x.MaxValue ?? int.MaxValue).WithMessage("MinValue must be less than or equal to MaxValue.");

        RuleFor(x => x.MaxValue)
            .GreaterThanOrEqualTo(x => x.MinValue ?? int.MinValue).WithMessage("MaxValue must be greater than or equal to MinValue.");

        RuleFor(x => x.RegexPattern)
            .MaximumLength(500).WithMessage("RegexPattern must not exceed 500 characters.");

        RuleFor(x => x.AllowedValues)
            .NotNull().WithMessage("AllowedValues must not be null.")
            .Must(av => av.All(v => !string.IsNullOrEmpty(v))).WithMessage("AllowedValues must not contain null or empty values.");
    }
}