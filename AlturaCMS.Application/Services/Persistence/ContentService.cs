using AlturaCMS.Application.Services.Persistence.Common;
using AlturaCMS.DataAccess;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Context;
using Microsoft.Extensions.Logging;

namespace AlturaCMS.Application.Services.Persistence
{
    public class ContentService(
        IUnitOfWork<ApplicationDbContext> unitOfWork,
        ILogger<BasePersistenceService<Content, ApplicationDbContext>> logger) : BasePersistenceService<Content, ApplicationDbContext>(unitOfWork, logger), IContentService
    {
        // Add any additional methods specific to this service to the interface and then implement them here
    }
}
