

namespace Catalog.API.Features.Product.Command.CreateProduct
{

    public record CreateProductCommand(string Name , string Description , decimal Price , List<string> Categories , string ImageFile) 
        : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);
    public class CreateProductCommandHandler(IDocumentSession documentSession) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var newProduct= new Catalog.API.Models.Product
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                Categories = command.Categories,
                ImageFile = command.ImageFile
            };
            // save db
            documentSession.Store(newProduct);

            await documentSession.SaveChangesAsync(cancellationToken);

            return default;
        }
    }
}
