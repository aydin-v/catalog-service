namespace CatalogService.Data.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public int CatalogId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Catalog Catalog { get; set; }
    }
}
