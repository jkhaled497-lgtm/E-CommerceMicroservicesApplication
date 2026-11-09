
using Catalog.API.Features.Product.Command.CreateProduct;

namespace Catalog.API.Features.Product.Command.DeleteProduct
{

    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/v1/product/{id:guid}", async (Guid id, ICommandHandler<DeleteProductCommand, Unit> handler) =>
            {
                var command = new DeleteProductCommand(id);
                await handler.Handle(command, CancellationToken.None);
                return Results.NoContent();
            })
                .WithName("Delete product")
                .Produces<Unit>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Delete a product")
                .WithDescription("Deletes a product with the specified ID.");
        }
    }
}
