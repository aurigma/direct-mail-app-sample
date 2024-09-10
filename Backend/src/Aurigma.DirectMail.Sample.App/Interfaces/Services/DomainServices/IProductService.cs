using Aurigma.DirectMail.Sample.App.Exceptions.Product;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Product;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;

/// <summary>
/// Application service for performing operations with products.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="model">Model to create a product.</param>
    /// <returns>The created product.</returns>
    Task<Product> CreateProductAsync(Product lineItem);

    /// <summary>
    /// Returns a product by id.
    /// </summary>
    /// <param name="id">ID of the product to search for.</param>
    /// <returns>The found product.</returns>
    /// <exception cref="ProductNotFoundException"> The product was not found.</exception>
    Task<Product> GetProductAsync(Guid id);

    /// <summary>
    /// Return all products by filter.
    /// </summary>
    /// <param name="filter">Request filter model.</param>
    /// <returns>All products satisfying the filter.</returns>
    Task<List<Product>> GetProductsAsync(ProductFilter filter = null);
}
