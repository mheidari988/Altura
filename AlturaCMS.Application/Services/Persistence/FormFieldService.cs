using AlturaCMS.Application.Services.Persistence.Common;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Repositories;

namespace AlturaCMS.Application.Services.Persistence;
public class FormFieldService(IUnitOfWork unitOfWork) : BasePersistenceService<FormField>(unitOfWork), IFormFieldService
{

    // Add any additional methods specific to FormFieldService to the IFormFieldService and implement it here
}