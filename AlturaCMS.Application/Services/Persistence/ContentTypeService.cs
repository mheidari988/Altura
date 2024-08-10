using AlturaCMS.Application.Services.Persistence.Common;
using AlturaCMS.DataAccess;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Context;

namespace AlturaCMS.Application.Services.Persistence;
public class ContentTypeService(IUnitOfWork<ApplicationDbContext> unitOfWork) : BasePersistenceService<Content, ApplicationDbContext>(unitOfWork), IContentTypeService
{
    // Add any additional methods specific to ContentTypeService to the IContentTypeService and implement it here
}
