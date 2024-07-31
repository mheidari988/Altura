using FluentValidation;

namespace AlturaCMS.Application.Features.Fields.Queries.GetFieldTypes;
public class GetFieldTypesValidator : AbstractValidator<GetFieldTypesQuery>
{
    public GetFieldTypesValidator()
    {
        // No need to validate
    }
}
