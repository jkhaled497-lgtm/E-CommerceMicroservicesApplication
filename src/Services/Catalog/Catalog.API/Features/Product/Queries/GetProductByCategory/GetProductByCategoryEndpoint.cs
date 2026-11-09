
namespace Catalog.API.Features.Product.Queries.GetProductByCategory
{
    public record GetProductByCategoryQuery(string search):IQuery<GetProductByCategoruResponse>;

    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/products/category/{search}", async (ISender sender, string search) =>
            {
                var query = new GetProductByCategoryQuery(search);
                var result = await sender.Send(query);
                return Results.Ok(result);
            })
                .WithName("Get product by category")
                .Produces<GetProductByCategoruResponse>()
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get products by category")
                .WithDescription("Get products by category");
        }
    }
}
