using AlturaCMS.Application.Services.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AlturaCMS.Application.Features.Fields.Queries.GetFields
{
    public class GetFieldsHandler(IFieldService fieldService, ILogger<GetFieldsHandler> logger) : IRequestHandler<GetFieldsQuery, GetFieldsResponse>
    {
        private readonly IFieldService _fieldService = fieldService ?? throw new ArgumentNullException(nameof(fieldService));
        private readonly ILogger<GetFieldsHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<GetFieldsResponse> Handle(GetFieldsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetFieldsQuery");

            // Step 1: Validate the request
            var validator = new GetFieldsValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validation failed for GetFieldsQuery. Errors: {ValidationErrors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }

            // Step 2: Fetch the fields from the database
            try
            {
                _logger.LogInformation("Fetching all fields for GetFieldsQuery");
                var fields = await _fieldService.GetAllAsync();

                _logger.LogInformation("Fetched {FieldCount} fields successfully", fields.Count);

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while handling GetFieldsQuery");
                throw;
            }
        }
    }
}
