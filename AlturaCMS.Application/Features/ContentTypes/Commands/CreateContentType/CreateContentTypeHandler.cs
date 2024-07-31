using AlturaCMS.Application.Services.Persistence;
using AlturaCMS.Domain.Entities;
using FluentValidation;
using MediatR;

namespace AlturaCMS.Application.Features.ContentTypes.Commands.CreateContentType;
public class CreateContentTypeHandler(
    IContentTypeService contentTypeService,
    IFieldService fieldService,
    IDynamicTableService dynamicTableService) : IRequestHandler<CreateContentTypeCommand, CreateContentTypeResponse>
{
    public async Task<CreateContentTypeResponse> Handle(CreateContentTypeCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateContentTypeValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var contentType = new ContentType
        {
            Name = request.Name,
            Fields = []
        };

        foreach (var fieldDto in request.Fields)
        {
            var field = await fieldService.GetByIdAsync(fieldDto.FieldId);
            if (field != null)
            {
                contentType.Fields.Add(new ContentTypeField
                {
                    FieldId = field.Id,
                    Field = field
                });
            }
        }

        var createdContentType = await contentTypeService.CreateAsync(contentType) ?? throw new Exception("Failed to create content type");
        dynamicTableService.CreateTable(createdContentType);

        return new CreateContentTypeResponse
        {
            Id = createdContentType.Id,
            Name = createdContentType.Name,
            Fields = createdContentType.Fields.Select(f => new ContentTypeFieldDto
            {
                FieldId = f.FieldId ?? Guid.Empty,
                FieldName = f.Field?.Name?.ToString() ?? string.Empty
            }).ToList()
        };
    }
}