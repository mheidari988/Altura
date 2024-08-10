using AlturaCMS.Application.Services.Persistence.Common;
using AlturaCMS.DataAccess;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Context;

namespace AlturaCMS.Application.Services.Persistence;
public class FieldService(IUnitOfWork<ApplicationDbContext> unitOfWork) : BasePersistenceService<ContentField, ApplicationDbContext>(unitOfWork), IFieldService
{
}
