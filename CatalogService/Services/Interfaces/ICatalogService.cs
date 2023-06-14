using CatalogService.Data.Entities;

namespace CatalogService.Services.Interfaces
{
    public interface ICatalogService
    {
        Task AddCatalog(Catalog catalog);
        Task<List<Catalog>> GetAllCatalogs();
        Task<Catalog> GetCatalogById(int CatalogId);
        Task<Catalog> UpdateCatalog(Catalog catalog);
        Task DeleteCatalogById(int id);
    }
}
