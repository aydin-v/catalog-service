using CatalogService.Controllers;
using CatalogService.Data;
using CatalogService.Data.Entities;
using CatalogService.Models;
using CatalogService.Services.Implementations;
using CatalogService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogServiceTests
{
    public class ControllerTests
    {
        private static DbContextOptions<ServiceDbContext> dbContextOptions = new DbContextOptionsBuilder<ServiceDbContext>()
                    .UseInMemoryDatabase(databaseName: "serviceDBTest")
                    .Options;

        ServiceDbContext dbContext;
        IItemService itemService;
        ICatalogService catalogService;

        [OneTimeSetUp]
        public void Setup()
        {
            dbContext = new ServiceDbContext(dbContextOptions);
            dbContext.Database.EnsureCreated();

            SeedDatabase();
            itemService = new ItemService(dbContext);
            catalogService = new CatalogService.Services.Implementations.CatalogService(dbContext);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            dbContext.Database.EnsureDeleted();
        }

        private void SeedDatabase()
        {
            var catalogs = new List<Catalog>()
            {
                new Catalog
                {
                    Id = 1,
                    Name = "Fast Food"
                },
                new Catalog
                {
                    Id = 2,
                    Name = "Dairy"
                },
                new Catalog
                {
                    Id = 3,
                    Name = "Fruit"
                }
            };
            dbContext.Catalogs.AddRange(catalogs);

            var items = new List<Item>()
            {
                new Item
                {
                    Id = 1,
                    Name = "Burger",
                    Description = "Burger with beef",
                    CatalogId = 1,
                },
                new Item
                {
                    Id = 2,
                    Name = "Chicken burger",
                    Description = "Burger with chicken",
                    CatalogId = 1,
                },
                new Item
                {
                    Id = 3,
                    Name = "Milk",
                    Description = "Cow milk",
                    CatalogId = 2,
                },
                new Item
                {
                    Id = 4,
                    Name = "Cheese",
                    Description = "Goat cheese",
                    CatalogId = 2,
                },
                new Item
                {
                    Id = 5,
                    Name = "Apple",
                    Description = "Apple",
                    CatalogId = 3,
                },
                new Item
                {
                    Id = 6,
                    Name = "Mango",
                    Description = "Mango",
                    CatalogId = 3,
                }
            };
            dbContext.Items.AddRange(items);

            dbContext.SaveChanges();
        }

        [Test]
        public void Get_All_Items()
        {
            var itemController = new ItemController(itemService);

            var actionResult = itemController.GetAllItems().Result;

            OkObjectResult? okResult = actionResult as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            Assert.That(okResult.Value, Is.EqualTo(dbContext.Items.ToList()));
        }


        [Test]
        [TestCase(2)]
        public void Get_Item_By_Id(int id)
        {
            var itemController = new ItemController(itemService);

            var actionResult = itemController.GetItemById(id).Result;

            OkObjectResult? okResult = actionResult as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            var item = okResult.Value as Item;

            Assert.That(item?.Name, Is.EqualTo("Chicken burger"));
        }

        [Test]
        [TestCase(7, "Peach", 3)]
        public void Add_Item(int id, string itemName, int catalogId)
        {
            var itemController = new ItemController(itemService);

            var item = new Item
            {
                Id = id,
                Name = itemName,
                CatalogId = catalogId,
            };
            var actionResult = itemController.AddItem(item).Result;

            OkResult? okResult = actionResult as OkResult;

            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        [TestCase(5, "Pear", "Pear", 1)]
        public void Update_Item_By_Id(int id, string itemName, string description, int catalogId)
        {
            var itemController = new ItemController(itemService);

            var item = new Item
            {
                Id = id,
                Name = itemName,
                Description = description,
                CatalogId = catalogId,
            };

            var actionResult = itemController.UpdateItem(item).Result;

            OkObjectResult? okResult = actionResult as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            var itemUpdated = okResult.Value as Item;

            Assert.That(itemUpdated?.Name, Is.EqualTo("Pear"));
        }

        [Test]
        [TestCase(7)]
        public void Delete_Item_By_Id(int id)
        {
            var itemController = new ItemController(itemService);

            var actionResult = itemController.DeleteItemById(id).Result;

            OkResult? okResult = actionResult as OkResult;

            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        [TestCase(2)]
        public void Get_Item_By_CatalogId(int catalogId)
        {
            var itemController = new ItemController(itemService);

            var result = itemController.GetItemsByCatalogId(catalogId).Result as OkObjectResult;

            Assert.That((result?.Value as List<Item>)?.Count, Is.EqualTo(2));
        }

        [Test]
        [TestCase(3, 2)]
        public void Get_Items_By_Paging(int page, int pageSize)
        {
            var itemController = new ItemController(itemService);

            var pageParm = new PagingParams
            {
                PageSize = pageSize,
                PageNumber = page
            };

            var actionResult = itemController.GetAllItemsWithPaging(pageParm).Result;

            OkObjectResult? okResult = actionResult as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            var result = okResult.Value as List<Item>;

            Assert.That(result?.Count(), Is.EqualTo(2));
        }

        //CatalogController-Tests

        [Test]
        public void Get_All_Catalogs()
        {
            var catalogController = new CatalogController(catalogService);

            var actionResult = catalogController.GetAllCalalogs().Result;

            OkObjectResult? okResult = actionResult as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            Assert.That(okResult.Value, Is.EqualTo(dbContext.Catalogs.ToList()));
        }

        [Test]
        [TestCase(4, "Bakery")]
        public void Add_Catalog(int id, string name)
        {
            var catalogController = new CatalogController(catalogService);

            var catalog = new Catalog
            {
                Id = id,
                Name = name
            };
            var actionResult = catalogController.AddCatalog(catalog).Result;

            OkResult? okResult = actionResult as OkResult;

            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }

        [Test]
        [TestCase(4)]
        public void Get_Catalog_By_Id(int id)
        {
            var catalogController = new CatalogController(catalogService);

            var actionResult = catalogController.GetCatalogById(id).Result;

            OkObjectResult? okResult = actionResult as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            var catalog = okResult.Value as Catalog;

            Assert.That(catalog?.Name, Is.EqualTo("Bakery"));
        }

        [Test]
        [TestCase(1, "APA")]
        public void Update_Catalog_By_Id(int id, string name)
        {
            var catalogController = new CatalogController(catalogService);

            var catalog = new Catalog
            {
                Id = id,
                Name = name
            };

            var actionResult = catalogController.UpdateCatalog(catalog).Result;

            OkObjectResult? okResult = actionResult as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));

            var catalogUpdated = okResult.Value as Catalog;

            Assert.That(catalogUpdated?.Id, Is.EqualTo(1));

        }

        [Test]
        [TestCase(6)]
        public void Delete_Catalog_By_Id(int id)
        {
            var catalogController = new CatalogController(catalogService);

            var actionResult = catalogController.DeleteCatalogById(id).Result;

            OkResult? okResult = actionResult as OkResult;

            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
        }
    }
}