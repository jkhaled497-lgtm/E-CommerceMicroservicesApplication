
namespace Catalog.API.Features.Product.Command.UpdateProduct
{
    public record UpdateProductCommand(Guid id , string Name, string Description, decimal Price, List<string> Categories, string ImageFile) : ICommand<Unit>;

    //public record UpdateProductResponse(ProductDTO ProductDTO);
    public class UpdateProductHandler(IDocumentSession documentSession) : ICommandHandler<UpdateProductCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var currentProduct = await documentSession.Query<Products>().FirstOrDefaultAsync(s => s.Id == request.id);

            if (currentProduct == null)
            {
                throw new ProductNotFoundException();
            }
            request.Adapt(currentProduct); /////////// source.destintion 

            documentSession.Update(currentProduct);

            await documentSession.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
