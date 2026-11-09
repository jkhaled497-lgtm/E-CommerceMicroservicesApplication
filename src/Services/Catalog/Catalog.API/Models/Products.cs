namespace Catalog.API.Models
{
    public class Products
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public List<string> Categories  { get; set; } = new ();
        public string ImageFile { get; set; }
    }
}
