using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Models.Product;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Product;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;

/// <summary>
/// Application service for performing operations with products.
/// </summary>
public interface IProductAppService
{
    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="model">Model to create a product.</param>
    /// <returns>The created product.</returns>
    Task<Product> CreateProductAsync(ProductCreationModel model);

    /// <summary>
    /// Returns a product by id.
    /// </summary>
    /// <param name="id">ID of the product to search for.</param>
    /// <returns>The found product.</returns>
    /// <exception cref="NotFoundAppException"> The product was not found.</exception>
    Task<Product> GetProductByIdAsync(Guid id);

    /// <summary>
    /// Return all products by filter.
    /// </summary>
    /// <param name="model">Request filter model.</param>
    /// <returns>All products satisfying the filter.</returns>
    Task<IEnumerable<Product>> GetProductsAsync(ProductRequestModel model);
}
