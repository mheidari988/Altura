using AlturaCMS.Application.Services.Persistence.Common;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Repositories;

namespace AlturaCMS.Application.Services.Persistence;
public class ContentTypeService(IUnitOfWork unitOfWork) : BasePersistenceService<ContentType>(unitOfWork), IContentTypeService
{

    // Add any additional methods specific to ContentTypeService to the IContentTypeService and implement it here
}
