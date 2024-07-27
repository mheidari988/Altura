using AlturaCMS.Application.Services.Persistence.Common;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Repositories;

namespace AlturaCMS.Application.Services.Persistence;
public class FormService(IUnitOfWork unitOfWork) : BasePersistenceService<Form>(unitOfWork), IFormService
{

    // Add any additional methods specific to FormService to the IFormService and implement it here
}