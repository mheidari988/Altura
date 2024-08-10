using AlturaCMS.Application.Services.Persistence;
using AlturaCMS.DataAccess;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Context;
using FluentValidation;
using MediatR;

namespace AlturaCMS.Application.Features.Contents.Commands.CreateContent
{
    public class CreateContentHandler(
        IContentTypeService contentTypeService,
        IDynamicTableService dynamicTableService,
        IUnitOfWork<ApplicationDbContext> unitOfWork) : IRequestHandler<CreateContentCommand, CreateContentResponse>
    {
        public async Task<CreateContentResponse> Handle(CreateContentCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateContentValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await unitOfWork.BeginTransactionAsync();

            try
            {
                // Step 1: Initialize the Content entity
                var contentType = new Content
                {
                    Name = request.Name,
                    ContentFields = [] // Initialize with an empty list
                };

                // Normalize string inputs
                NormalizeStringInputs(request);

                // Add default metadata fields for content type
                request.Fields.AddRange(ApplicationShared.GetDefaultFields());

                // Step 2: Create ContentField entities and add them to the Content entity
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

                // Step 3: Save the Content entity along with its associated ContentFields
                var createdContentType = await contentTypeService.CreateAsync(contentType)
                    ?? throw new Exception("Failed to create content type");

                // Step 4: Create the dynamic table if the content type was created successfully
                var isCreated = await dynamicTableService.CreateTableAsync(createdContentType);

                if (createdContentType != null && isCreated)
                {
                    await unitOfWork.CompleteAsync();
                    await unitOfWork.CommitTransactionAsync();
                }
                else
                {
                    await unitOfWork.RollbackTransactionAsync();
                    throw new Exception("Transaction failed: either content type creation or table creation failed.");
                }

                return new CreateContentResponse
                {
                    Id = createdContentType.Id,
                    Name = createdContentType.Name,
                    ContentFields = createdContentType.ContentFields
                        .Where(f => !ApplicationShared.GetDefaultFields() // Filter out default fields from the response
                            .Any(df => df.Name == f.Name && df.DisplayName == f.DisplayName))
                        .Select(f => new ContentFieldDto
                        {
                            FieldName = f.Name?.ToString() ?? string.Empty,
                        }).ToList()
                };
            }
            catch (Exception)
            {
                await unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        /// <summary>
        /// Normalize string inputs by trimming leading and trailing whitespace characters
        /// </summary>
        /// <param name="request">
        /// The <see cref="CreateContentCommand"/> request object to normalize.
        /// </param>
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
