using AlturaCMS.Domain.Entities;

namespace AlturaCMS.Application.Services.Persistence;
public interface IDynamicTableService
{
    void AlterTable(ContentType contentType);
    void CreateTable(ContentType contentType);
}