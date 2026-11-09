namespace Catalog.API.Features.Product.Mapper
{
    public record ProductMapper
    {

    }
    public class ProductDTO 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public List<string> Categories = new List<string>();
        public string ImageFile { get; set; }
    }

}
