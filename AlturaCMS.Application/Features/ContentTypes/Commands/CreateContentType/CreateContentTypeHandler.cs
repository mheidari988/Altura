using AlturaCMS.Application.Services.Persistence;
using AlturaCMS.DataAccess;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Context;
using FluentValidation;
using MediatR;

namespace AlturaCMS.Application.Features.ContentTypes.Commands.CreateContentType
{
    public class CreateContentTypeHandler : IRequestHandler<CreateContentTypeCommand, CreateContentTypeResponse>
    {
        private readonly IContentTypeService _contentTypeService;
        private readonly IFieldService _fieldService;
        private readonly IDynamicTableService _dynamicTableService;
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

        public CreateContentTypeHandler(
            IContentTypeService contentTypeService,
            IFieldService fieldService,
            IDynamicTableService dynamicTableService,
            IUnitOfWork<ApplicationDbContext> unitOfWork)
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

                // Normalize string inputs
                NormalizeStringInputs(request);

                // Add default metadata fields for content type
                request.Fields.AddRange(ApplicationShared.GetDefaultFields());

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
                        ReferenceDisplayFieldName = fieldDto.ReferenceDisplayFieldName,
                        ReferenceTableName = fieldDto.ReferenceTableName
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
                    Fields = createdContentType.Fields
                    // Filter out default fields from the response
                    .Where(f => !ApplicationShared.GetDefaultFields()
                        .Any(df => df.Name == f.Field?.Name && df.DisplayName == f.Field.DisplayName))
                    .Select(f => new ContentTypeFieldDto
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

        /// <summary>
        /// Normalize string inputs by trimming leading and trailing whitespace characters
        /// </summary>
        /// <param name="request">
        /// The <see cref="CreateContentTypeCommand"/> request object to normalize.
        /// </param>
        private static void NormalizeStringInputs(CreateContentTypeCommand request)
        {
            request.Name = request.Name.Trim();
            request.Fields.ForEach(f => f.Name = f.Name.Trim());
            request.Fields.ForEach(f => f.DisplayName = f.DisplayName.Trim());
            request.Fields.ForEach(f => f.ReferenceTableName = f.ReferenceTableName?.Trim());
            request.Fields.ForEach(f => f.ReferenceDisplayFieldName = f.ReferenceDisplayFieldName?.Trim());
        }
    }
}
