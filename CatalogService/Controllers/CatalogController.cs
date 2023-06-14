using CatalogService.Data.Entities;
using CatalogService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers
{
    [Route("api")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpPost]
        [Route("catalog")]
        public async Task<IActionResult> AddCatalog([FromBody] Catalog catalog)
        {
            await _catalogService.AddCatalog(catalog);

            return Ok();
        }

        [HttpGet]
        [Route("catalogs")]
        public async Task<IActionResult> GetAllCalalogs()
        {
            var catalogs = await _catalogService.GetAllCatalogs();
            return catalogs is not null
                ? Ok(catalogs)
                : BadRequest();
        }

        [HttpGet]
        [Route("catalog/{id}")]
        public async Task<IActionResult> GetCatalogById(int id)
        {
            var catalog = await _catalogService.GetCatalogById(id);

            return catalog is not null
                ? Ok(catalog)
                : BadRequest();
        }

        [HttpPut]
        [Route("catalog")]
        public async Task<IActionResult> UpdateCatalog([FromBody] Catalog catalog)
        {
            var updatedCatalog = await _catalogService.UpdateCatalog(catalog);

            return updatedCatalog is not null
                ? Ok(updatedCatalog)
                : BadRequest();
        }

        [HttpDelete]
        [Route("catalog/{id}")]
        public async Task<IActionResult> DeleteCatalogById(int id)
        {
            await _catalogService.DeleteCatalogById(id);

            return Ok();
        }
    }
}
