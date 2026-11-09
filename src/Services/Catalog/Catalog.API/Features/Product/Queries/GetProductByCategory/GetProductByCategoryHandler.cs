
namespace Catalog.API.Features.Product.Queries.GetProductByCategory
{
    public record GetProductByCategoruResponse(IEnumerable<Products> products);
    public class GetProductByCategoryHandler(IDocumentSession documentSession) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoruResponse>
    {
        public async Task<GetProductByCategoruResponse> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            var allProduct = await documentSession.Query<Products>()
                .Where(s=>s.Categories.Contains(request.search)).ToListAsync();

            //var result = allProduct.Adapt<GetProductByCategoruResponse>();

            return new GetProductByCategoruResponse(allProduct);
        }
    }
}
