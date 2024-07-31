using AlturaCMS.Application.Services.Persistence;
using FluentValidation;
using MediatR;

namespace AlturaCMS.Application.Features.Fields.Queries.GetFields;
public class GetFieldsHandler(IFieldService fieldService) : IRequestHandler<GetFieldsQuery, GetFieldsResponse>
{
    public async Task<GetFieldsResponse> Handle(GetFieldsQuery request, CancellationToken cancellationToken)
    {
        var validator = new GetFieldsValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var fields = await fieldService.GetAllAsync();

        return new GetFieldsResponse
        {
            Fields = fields.Select(f => new FieldDto
            {
                Id = f.Id,
                Name = f.Name,
                FieldType = f.FieldType,
                IsRequired = f.IsRequired,
                MinLength = f.MinLength,
                MaxLength = f.MaxLength,
                MinValue = f.MinValue,
                MaxValue = f.MaxValue,
                RegexPattern = f.RegexPattern,
                AllowedValues = f.AllowedValues,
            }).ToList()
        };
    }
}
