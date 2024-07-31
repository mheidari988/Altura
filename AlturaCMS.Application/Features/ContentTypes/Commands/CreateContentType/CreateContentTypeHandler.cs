using AlturaCMS.Application.Services.Persistence;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Repositories;
using FluentValidation;
using MediatR;

namespace AlturaCMS.Application.Features.ContentTypes.Commands.CreateContentType
{
    public class CreateContentTypeHandler : IRequestHandler<CreateContentTypeCommand, CreateContentTypeResponse>
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IFieldService _fieldService;
        private readonly IDynamicTableService _dynamicTableService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateContentTypeHandler(
            IContentTypeService contentTypeService,
            IFieldService fieldService,
            IDynamicTableService dynamicTableService,
            IUnitOfWork unitOfWork)
        {
            _contentTypeService = contentTypeService;
            _fieldService = fieldService;
            _dynamicTableService = dynamicTableService;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateContentTypeResponse> Handle(CreateContentTypeCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateContentTypeValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var contentType = new ContentType
                {
                    Name = request.Name,
                    Fields = []
                };

                // Slug is a required field for all content types
                request.Fields.Insert(0, new CreateContentTypeFieldDto()
                {
                    Name = "Slug",
                    DisplayName = "Slug",
                    FieldType = FieldType.Text,
                    IsRequired = true,
                    IsUnique = true,
                    MaxLength = 500,
                    RegexPattern = null
                });

                foreach (var fieldDto in request.Fields)
                {
                    var field = await _fieldService.CreateAsync(new Field
                    {
                        Name = fieldDto.Name,
                        DisplayName = fieldDto.DisplayName,
                        FieldType = fieldDto.FieldType,
                        IsRequired = fieldDto.IsRequired,
                        IsUnique = fieldDto.IsUnique,
                        AllowedValues = fieldDto.AllowedValues,
                        MaxLength = fieldDto.MaxLength,
                        MinLength = fieldDto.MinLength,
                        MaxValue = fieldDto.MaxValue,
                        MinValue = fieldDto.MinValue,
                        RegexPattern = fieldDto.RegexPattern,
                    });

                    if (field != null)
                    {
                        contentType.Fields.Add(new ContentTypeField
                        {
                            FieldId = field.Id,
                            Field = field
                        });
                    }
                }

                var createdContentType = await _contentTypeService.CreateAsync(contentType)
                    ?? throw new Exception("Failed to create content type");

                var isCreated = await _dynamicTableService.CreateTableAsync(createdContentType);

                if (createdContentType != null && isCreated)
                {
                    await _unitOfWork.CompleteAsync();
                    await _unitOfWork.CommitTransactionAsync();
                }
                else
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new Exception("Transaction failed: either content type creation or table creation failed.");
                }

                return new CreateContentTypeResponse
                {
                    Id = createdContentType.Id,
                    Name = createdContentType.Name,
                    Fields = createdContentType.Fields.Select(f => new ContentTypeFieldDto
                    {
                        FieldId = f.FieldId ?? Guid.Empty,
                        FieldName = f.Field?.Name?.ToString() ?? string.Empty,
                    }).ToList()
                };
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
