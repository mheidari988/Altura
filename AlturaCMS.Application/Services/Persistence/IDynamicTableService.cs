using AlturaCMS.Domain.Entities;

namespace AlturaCMS.Application.Services.Persistence;
public interface IDynamicTableService
{
    ValueTask<bool> CreateTableAsync(Content contentType);
}