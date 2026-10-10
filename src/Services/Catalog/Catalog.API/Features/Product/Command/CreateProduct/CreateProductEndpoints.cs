
namespace Catalog.API.Features.Product.Command.CreateProduct
{
    public record CreateProductRequest(string Name, string Description, decimal Price, List<string> Categories, string ImageFile);
    public record CreateProductResponse(Guid Id);
    public class CreateProductEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/products", async (CreateProductRequest request, ISender sender) =>
            {
                var commandobject = request.Adapt<CreateProductCommand>();
                var result = await sender.Send(commandobject);

                var response = result.Adapt<CreateProductResponse>();

                return Results.Created($"/api/prducts{response.Id}", response);
            })
                .WithName("CreateProduct")
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create a new product")
                .WithDescription("Creates a new product with the specified details and returns the created product's ID.");
        }
    }
}
