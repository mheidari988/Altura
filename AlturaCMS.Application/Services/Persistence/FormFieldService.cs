using AlturaCMS.Application.Services.Persistence.Common;
using AlturaCMS.DataAccess;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Context;
using Microsoft.Extensions.Logging;

namespace AlturaCMS.Application.Services.Persistence;
public class FormFieldService(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    ILogger<BasePersistenceService<FormField, ApplicationDbContext>> logger) : BasePersistenceService<FormField, ApplicationDbContext>(unitOfWork, logger), IFormFieldService
{
    // Add any additional methods specific to this service to the interface and then implement them here
}