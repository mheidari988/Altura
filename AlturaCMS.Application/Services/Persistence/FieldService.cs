using AlturaCMS.Application.Services.Persistence.Common;
using AlturaCMS.DataAccess;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Context;
using Microsoft.Extensions.Logging;

namespace AlturaCMS.Application.Services.Persistence;
public class FieldService(
    IUnitOfWork<ApplicationDbContext> unitOfWork,
    ILogger<BasePersistenceService<ContentField, ApplicationDbContext>> logger) : BasePersistenceService<ContentField, ApplicationDbContext>(unitOfWork, logger), IFieldService
{
    // Add any additional methods specific to this service to the interface and then implement them here
}
