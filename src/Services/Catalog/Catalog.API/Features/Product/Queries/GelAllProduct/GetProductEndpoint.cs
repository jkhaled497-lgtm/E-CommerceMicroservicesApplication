namespace Catalog.API.Features.Product.Queries.GelAllProduct
{
    public record GetProductResponse(IEnumerable<Products> Products);

    public record GetProductByIdResponse(ProductDTO ProductDTO);
    public class GetProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/products", async (ISender sender) =>
            {
                var query = new GetAllProductsQuery();
                var result = await sender.Send(query);
                var response = new GetProductResponse(result.Products);
                return Results.Ok(response);
            })
            .WithName("GetAllProducts")
            .Produces<GetProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get all products")
            .WithDescription("Retrieves a list of all products available in the catalog.");

            app.MapGet("/api/products/{id}", async (ISender sender, Guid id) =>
            {
                var query = new GetProductQueryById() { Id = id };

                var result = await sender.Send(query);
                return Results.Ok(result);
            })
                .WithName("GetSpecificProduct")
                .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Product by Id")
                .WithDescription("Retrieves a specific product by its unique identifier.");

        }
    
    }
}
