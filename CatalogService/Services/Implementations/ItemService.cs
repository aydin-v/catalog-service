using CatalogService.Data;
using CatalogService.Data.Entities;
using CatalogService.Models;
using CatalogService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Services.Implementations
{
    public class ItemService : IItemService
    {
        private ServiceDbContext _dbContext;
        public ItemService(ServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddItem(Item item)
        {
            _dbContext.Items.Add(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Item>> GetAllItems() => await _dbContext.Items.ToListAsync();

        public async Task<List<Item>> GetAllItemsWithPaging(PagingParams pageingParams)
        {
            return await _dbContext.Items
                .Skip((pageingParams.PageNumber - 1) * pageingParams.PageSize)
                .Take(pageingParams.PageSize)
                .ToListAsync();

        }

        public async Task<Item> GetItemById(int itemId) => await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == itemId);

        public async Task<List<Item>> GetItemsByCatalogId(int catalogId) => await _dbContext.Items.Where(x => x.CatalogId == catalogId).ToListAsync();

        public async Task<Item> UpdateItem(Item item)
        {
            var _item = await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == item.Id);

            if (_item != null)
            {
                _item.Name = item.Name;
                _item.CatalogId = item.CatalogId;
                _item.Description = item.Description;

                await _dbContext.SaveChangesAsync();
            }

            return _item!;
        }

        public async Task DeleteItemById(int id)
        {
            var _item = await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == id);
            if (_item != null)
            {
                _dbContext.Items.Remove(_item);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
