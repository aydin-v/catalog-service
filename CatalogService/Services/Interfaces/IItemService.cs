using CatalogService.Data.Entities;
using CatalogService.Models;

namespace CatalogService.Services.Interfaces
{
    public interface IItemService
    {
        Task AddItem(Item item);
        Task<List<Item>> GetAllItems();
        Task<List<Item>> GetAllItemsWithPaging(PagingParams pageingParams);   
        Task<Item> GetItemById(int itemId);
        Task<List<Item>> GetItemsByCatalogId(int catalogId);
        Task<Item> UpdateItem(Item item);
        Task DeleteItemById(int id);
    }
}
