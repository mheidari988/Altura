using AlturaCMS.Application.Services.Persistence.Common;
using AlturaCMS.Domain.Entities;
using AlturaCMS.Persistence.Repositories;

namespace AlturaCMS.Application.Services.Persistence;
public class FieldService(IUnitOfWork unitOfWork) : BasePersistenceService<Field>(unitOfWork), IFieldService
{
}
