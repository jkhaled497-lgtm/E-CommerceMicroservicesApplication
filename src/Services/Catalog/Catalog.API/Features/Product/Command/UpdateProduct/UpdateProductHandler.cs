
namespace Catalog.API.Features.Product.Command.UpdateProduct
{
    public record UpdateProductCommand(Guid id , string Name, string Description, decimal Price, List<string> Categories, string ImageFile) : ICommand<Unit>;

    //public record UpdateProductResponse(ProductDTO ProductDTO);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.id).NotEmpty().WithMessage("Id is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.")
                .Length(2, 18).WithMessage("Name should be greater than 2 an less than 18 characters");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.Categories).NotEmpty().WithMessage("At least one category is required.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image file is required.");
        }
    }
    public class UpdateProductHandler(IDocumentSession documentSession) : ICommandHandler<UpdateProductCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var currentProduct = await documentSession.Query<Products>().FirstOrDefaultAsync(s => s.Id == request.id);

            if (currentProduct == null)
            {
                throw new ProductNotFoundException(request.id);
            }
            request.Adapt(currentProduct); /////////// source.destintion 

            documentSession.Update(currentProduct);

            await documentSession.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
