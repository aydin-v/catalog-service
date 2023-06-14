namespace CatalogService.Data.Entities
{
    public class Catalog
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
