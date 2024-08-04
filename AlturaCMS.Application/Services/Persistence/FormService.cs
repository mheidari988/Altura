using AlturaCMS.Application.Services.Persistence.Common;
using AlturaCMS.DataAccess;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Context;

namespace AlturaCMS.Application.Services.Persistence;
public class FormService(IUnitOfWork<ApplicationDbContext> unitOfWork) : BasePersistenceService<Form, ApplicationDbContext>(unitOfWork), IFormService
{

    // Add any additional methods specific to FormService to the IFormService and implement it here
}