using FluentValidation;

namespace AlturaCMS.Application.Features.Fields.Queries.GetFields;
public class GetFieldsValidator : AbstractValidator<GetFieldsQuery>
{
    public GetFieldsValidator()
    {
        // No need to validate
    }
}
