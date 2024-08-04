using AlturaCMS.Application.Services.Persistence.Common;
using AlturaCMS.DataAccess;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Context;

namespace AlturaCMS.Application.Services.Persistence;
public class FormFieldService(IUnitOfWork<ApplicationDbContext> unitOfWork) : BasePersistenceService<FormField, ApplicationDbContext>(unitOfWork), IFormFieldService
{

    // Add any additional methods specific to FormFieldService to the IFormFieldService and implement it here
}