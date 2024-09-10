using Aurigma.DirectMail.Sample.App.Exceptions.Product;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Storages;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Product;

namespace Aurigma.DirectMail.Sample.App.Services.DomainServices;

public class ProductService(IProductRepository productRepository, ICategoryService categoryService)
    : IProductService
{
    private readonly IProductRepository _repository = productRepository;
    private readonly ICategoryService _categoryService = categoryService;

    public async Task<Product> CreateProductAsync(Product product)
    {
        if (product.CategoryId.HasValue)
            await ValidateCategory(product.CategoryId.Value);

        return await _repository.CreateProductAsync(product);
    }

    public async Task<Product> GetProductAsync(Guid id)
    {
        return await _repository.GetProductByIdAsReadOnlyAsync(id)
            ?? throw new ProductNotFoundException(
                id,
                $"The product with identifier '{id}' was not found"
            );
    }

    public async Task<List<Product>> GetProductsAsync(ProductFilter filter = null)
    {
        if (filter is null)
            return await _repository.GetAllAsReadOnlyAsync();

        return await _repository.GetProductsByFilterAsync(filter);
    }

    private async Task ValidateCategory(Guid categoryId)
    {
        var category = await _categoryService.GetCategoryAsync(categoryId);
    }
}
