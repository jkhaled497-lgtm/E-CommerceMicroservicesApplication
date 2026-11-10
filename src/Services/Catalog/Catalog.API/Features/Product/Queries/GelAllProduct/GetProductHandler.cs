namespace Catalog.API.Features.Product.Queries.GelAllProduct
{
    public record GetAllProductsQuery() : IQuery<GetAllProductResult>;
    public record GetProductQueryById() : IQuery<GetProductByIdResult>
    {
        public Guid Id { get; set; }
    };

    public record GetProductByIdResult(ProductDTO ProductDTO);
    public record GetAllProductResult(IEnumerable<Products> Products);
    public class GetAllProductsQueryHandler(IQuerySession querySession)
        : IQueryHandler<GetAllProductsQuery, GetAllProductResult>
    {
        public async Task<GetAllProductResult> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {

            var allProducts = await querySession.Query<Products>().ToListAsync(cancellationToken);

            return new GetAllProductResult(allProducts);

        }
    }

    public class GetProductQueryByIdHandler(IQuerySession querySession) 
        : IQueryHandler<GetProductQueryById, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductQueryById request, CancellationToken cancellationToken)
        {
            var CurrentProduct = await querySession.Query<Products>().FirstOrDefaultAsync(s => s.Id == request.Id);

            if(CurrentProduct is null)
            {
                throw new ProductNotFoundException(request.Id);
            }
            var result = CurrentProduct.Adapt<ProductDTO>();

            return new GetProductByIdResult(result);
        }
    }
}
