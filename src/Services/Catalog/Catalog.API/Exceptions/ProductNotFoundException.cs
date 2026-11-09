namespace Catalog.API.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException() :base("Product are not found !")
        {
            
        }
    }
}
