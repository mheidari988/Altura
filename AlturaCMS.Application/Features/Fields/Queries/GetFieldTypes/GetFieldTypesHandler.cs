using AlturaCMS.Domain.Entities;
using FluentValidation;
using MediatR;

namespace AlturaCMS.Application.Features.Fields.Queries.GetFieldTypes;
public class GetFieldTypesHandler : IRequestHandler<GetFieldTypesQuery, GetFieldTypesResponse>
{
    public async Task<GetFieldTypesResponse> Handle(GetFieldTypesQuery request, CancellationToken cancellationToken)
    {
        var validator = new GetFieldTypesValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var fieldTypes = Enum.GetValues(typeof(FieldType))
            .Cast<FieldType>()
            .Select(ft => new FieldTypeDto
            {
                Id = (int)ft,
                Name = ft.ToString()
            })
            .ToList();

        return new GetFieldTypesResponse
        {
            FieldTypes = fieldTypes
        };
    }
}
