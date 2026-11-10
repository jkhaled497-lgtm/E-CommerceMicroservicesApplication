
namespace Catalog.API.Features.Product.Command.DeleteProduct
{
    public record DeleteProductCommand (Guid Id) : ICommand<Unit>;

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required.");
        }
    }
    public class DeleteProductHandler(IDocumentSession documentSession) : ICommandHandler<DeleteProductCommand, Unit>
    {
        public Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
           var currentProduct = documentSession.Load<Products>(request.Id);


            if(currentProduct is null)
            {
                throw new ProductNotFoundException(request.Id);
            }
            documentSession.Delete(currentProduct);
            documentSession.SaveChanges();
            return Task.FromResult(Unit.Value);
        }
    }
}
