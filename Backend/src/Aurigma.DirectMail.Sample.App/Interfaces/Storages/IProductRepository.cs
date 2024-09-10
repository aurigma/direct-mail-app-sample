using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Product;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Storages;

public interface IProductRepository
{
    Task<Product> GetProductByIdAsync(Guid id);
    Task<Product> GetProductByIdAsReadOnlyAsync(Guid id);

    Task<List<Product>> GetAllAsync();
    Task<List<Product>> GetAllAsReadOnlyAsync();
    Task<List<Product>> GetProductsByFilterAsync(ProductFilter filter);

    Task<Product> CreateProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task<Product> DeleteProductAsync(Guid id);
}
