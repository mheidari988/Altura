using AlturaCMS.Application.Services.Persistence;
using AlturaCMS.DataAccess;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Context;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AlturaCMS.Application.Features.Contents.Commands.CreateContent
{
    public class CreateContentHandler(
        IContentService contentTypeService,
        IDynamicTableService dynamicTableService,
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        ILogger<CreateContentHandler> logger) : IRequestHandler<CreateContentCommand, CreateContentResponse>
    {
        private readonly IContentService _contentTypeService = contentTypeService ?? throw new ArgumentNullException(nameof(contentTypeService));
        private readonly IDynamicTableService _dynamicTableService = dynamicTableService ?? throw new ArgumentNullException(nameof(dynamicTableService));
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<CreateContentHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<CreateContentResponse> Handle(CreateContentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateContentCommand for content: {ContentName}", request.Name);

            // Step 1: Validate the request
            var validator = new CreateContentValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for content: {ContentName}. Errors: {ValidationErrors}", request.Name, validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Step 2: Initialize the Content entity
                var contentType = new Content
                {
                    Name = request.Name,
                    ContentFields = [] // Initialize with an empty list
                };

                // Step 3: Normalize string inputs
                NormalizeStringInputs(request);

                // Add default metadata fields for content type
                request.Fields.AddRange(ApplicationShared.GetDefaultFields());

                _logger.LogDebug("Initialized content entity with {FieldCount} fields for content: {ContentName}", request.Fields.Count, request.Name);

                // Step 4: Create ContentField entities and add them to the Content entity
                foreach (var fieldDto in request.Fields)
                {
                    var field = new ContentField
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
                        ReferenceTableName = fieldDto.ReferenceTableName,
                    };

                    contentType.ContentFields.Add(field);
                }

                _logger.LogInformation("Attempting to create content with name: {ContentName}", request.Name);

                // Step 5: Save the Content entity along with its associated ContentFields
                var createdContentType = await _contentTypeService.CreateAsync(contentType)
                    ?? throw new Exception("Failed to create content type");

                _logger.LogInformation("Content created successfully with ID: {ContentId}", createdContentType.Id);

                // Step 6: Create the dynamic table if the content type was created successfully
                var isCreated = await _dynamicTableService.CreateTableAsync(createdContentType);

                if (createdContentType != null && isCreated)
                {
                    await _unitOfWork.CompleteAsync();
                    await _unitOfWork.CommitTransactionAsync();

                    _logger.LogInformation("Dynamic table created successfully for content: {ContentName}", createdContentType.Name);
                }
                else
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    _logger.LogError("Transaction failed: either content type creation or table creation failed.");
                    throw new Exception("Transaction failed: either content type creation or table creation failed.");
                }

                return new CreateContentResponse
                {
                    Id = createdContentType.Id,
                    Name = createdContentType.Name,
                    ContentFields = createdContentType.ContentFields
                        .Where(f => !ApplicationShared.GetDefaultFields()
                            .Any(df => df.Name == f.Name && df.DisplayName == f.DisplayName))
                        .Select(f => new ContentFieldDto
                        {
                            FieldName = f.Name ?? string.Empty,
                        }).ToList()
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "An error occurred while handling CreateContentCommand for content: {ContentName}", request.Name);
                throw;
            }
        }

        private static void NormalizeStringInputs(CreateContentCommand request)
        {
            request.Name = request.Name.Trim();
            request.Fields.ForEach(f => f.Name = f.Name.Trim());
            request.Fields.ForEach(f => f.DisplayName = f.DisplayName.Trim());
            request.Fields.ForEach(f => f.ReferenceTableName = f.ReferenceTableName?.Trim());
            request.Fields.ForEach(f => f.ReferenceDisplayFieldName = f.ReferenceDisplayFieldName?.Trim());
        }
    }
}
