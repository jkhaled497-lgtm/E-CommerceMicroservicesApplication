


using Buildingblocks.Exceptions;

namespace Catalog.API.Features.Product.Command.CreateProduct
{

    public record CreateProductCommand(string Name , string Description , decimal Price , List<string> Categories , string ImageFile) 
        : ICommand<CreateProductResult>;
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.Categories).NotEmpty().WithMessage("At least one category is required.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image file is required.");
        }
    }
    public record CreateProductResult(Guid Id);
    public class CreateProductCommandHandler(IDocumentSession documentSession , ILogger<CreateProductCommandHandler> logger) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("CreateProductCommandHandler.handler");
            var Id = new Guid();
            if (1 == 1)
            {
                IDictionary<string, string[]> errors = new Dictionary<string, string[]>
    {
        { "Key", new[] { "Keyx are not correct" } }
    };

                throw new BadRequestException(errors);
            }
            var newProduct= new Catalog.API.Models.Products
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

            return new CreateProductResult(newProduct.Id);
        }
    }
}
