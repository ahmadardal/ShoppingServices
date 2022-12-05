using Grpc.Core;
using InventoryService;

namespace InventoryService.Services;

public class ProductService : GetProductService.GetProductServiceBase
{
    private readonly ILogger<ProductService> _logger;
    private readonly InventoryContext db;
    public ProductService(ILogger<ProductService> logger, InventoryContext inventoryContext)
    {
        _logger = logger;
        db = inventoryContext;
    }

    public override async Task<ProductResponse> GetProduct(ProductRequest request, ServerCallContext context)
    {

        var response = new ProductResponse();

        var idGuid = new Guid(request.ProductId);

        var product = await db.Products.FindAsync(idGuid);

        if (product != null)
        {
            response.Id = product.id.ToString();
            response.Name = product.name;
            response.Image = product.image;
            response.Price = product.price;
            response.Category = product.category;
            response.Available = product.available;
        }

        return await Task.FromResult(response);
    }
}