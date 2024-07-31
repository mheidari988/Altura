using AlturaCMS.Domain.Entities;

namespace AlturaCMS.Application.Services.Persistence;
public interface IDynamicTableService
{
    Task CreateTableAsync(ContentType contentType);
}