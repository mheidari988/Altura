using AlturaCMS.Application.Services.Persistence.Common;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Repositories;

namespace AlturaCMS.Application.Services.Persistence;
public class ValidationRulesService(IUnitOfWork unitOfWork) : BasePersistenceService<ValidationRules>(unitOfWork), IValidationRulesService
{

    // Add any additional methods specific to ValidationRulesService to the IValidationRulesService and implement it here
}