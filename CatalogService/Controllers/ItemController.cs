using CatalogService.Data.Entities;
using CatalogService.Models;
using CatalogService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [Route("api")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost]
        [Route("item")]
        public async Task<IActionResult> AddItem([FromBody] Item item)
        {
            await _itemService.AddItem(item);
            return Ok();
        }

        [HttpGet]
        [Route("items")]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _itemService.GetAllItems();
            return Ok(items);
        }

        [HttpGet]
        [Route("item/{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var item = await _itemService.GetItemById(id);

            return item is not null
                ? Ok(item)
                : NotFound();
        }

        [HttpPut]
        [Route("item")]
        public async Task<IActionResult> UpdateItem([FromBody] Item item)
        {
            var updatedItem = await _itemService.UpdateItem(item);

            return updatedItem is not null
                ? Ok(updatedItem)
                : BadRequest();
        }

        [HttpDelete]
        [Route("item/{id}")]
        public async Task<IActionResult> DeleteItemById(int id)
        {
            await _itemService.DeleteItemById(id);
            return Ok();
        }

        [HttpGet]
        [Route("items/filter")]
        public async Task<IActionResult> GetItemsByCatalogId([FromQuery] int catalogId)
        {
            var items = await _itemService.GetItemsByCatalogId(catalogId);
            return Ok(items);
        }

        [HttpGet]
        [Route("items/pagination")]
        public async Task<IActionResult> GetAllItemsWithPaging([FromQuery] PagingParams pagingParams)
        {
            var filteredItem = await _itemService.GetAllItemsWithPaging(pagingParams);
            return filteredItem is not null
                ? Ok(filteredItem)
                : BadRequest();
        }
    }
}
