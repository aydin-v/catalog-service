using CatalogService.Data;
using CatalogService.Data.Entities;
using CatalogService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Services.Implementations
{
    public class CatalogService : ICatalogService
    {
        private ServiceDbContext _dbContext;
        public CatalogService(ServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddCatalog(Catalog catalog)
        {
            _dbContext.Catalogs.Add(catalog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Catalog>> GetAllCatalogs() => await _dbContext.Catalogs.ToListAsync();

        public async Task<Catalog> GetCatalogById(int CatalogId) => await _dbContext.Catalogs.FirstOrDefaultAsync(x => x.Id == CatalogId);

        public async Task<Catalog> UpdateCatalog(Catalog catalog)
        {
            var _catalog = await _dbContext.Catalogs.FirstOrDefaultAsync(x => x.Id == catalog.Id);

            if (_catalog != null)
            {
                _catalog.Name = catalog.Name;

                await _dbContext.SaveChangesAsync();
            }

            return _catalog!;
        }

        public async Task DeleteCatalogById(int id)
        {
            var _style = await _dbContext.Catalogs.FirstOrDefaultAsync(x => x.Id == id);
            if (_style != null)
            {
                _dbContext.Catalogs.Remove(_style);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
