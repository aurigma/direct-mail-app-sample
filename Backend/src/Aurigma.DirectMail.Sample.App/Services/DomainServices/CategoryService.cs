using Aurigma.DirectMail.Sample.App.Exceptions.Category;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Storages;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Category;

namespace Aurigma.DirectMail.Sample.App.Services.DomainServices;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        return await _categoryRepository.CreateCategoryAsync(category);
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await _categoryRepository.GetAllAsReadOnlyAsync();
    }

    public async Task<Category> GetCategoryAsync(Guid id)
    {
        return await _categoryRepository.GetCategoryByIdAsync(id)
            ?? throw new CategoryNotFoundException(
                id,
                $"The category with identifier '{id}' was not found"
            );
    }
}
