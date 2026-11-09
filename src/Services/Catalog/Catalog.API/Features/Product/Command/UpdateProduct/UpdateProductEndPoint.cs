using Catalog.API.Features.Product.Command.CreateProduct;
using Marten.Linq.SoftDeletes;

namespace Catalog.API.Features.Product.Command.UpdateProduct
{
    public record UpdateProductRequest( Guid id, string Name, string Description, decimal Price, List<string> Categories, string ImageFile);

    public class UpdateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
           app.MapPut("/api/products/{id}", async (ISender sender, Guid id, UpdateProductRequest request) =>
            {
                //var command = new UpdateProductCommand(id, request.Name, request.Description, request.Price, request.Categories, request.ImageFile);
                var command = request.Adapt<UpdateProductCommand>() with { id = id };
                var result = await sender.Send(command);
                
                return Results.Created();
            })
            .WithName("Update product")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update product")
            .WithDescription("Update product");
        }
    }
}
