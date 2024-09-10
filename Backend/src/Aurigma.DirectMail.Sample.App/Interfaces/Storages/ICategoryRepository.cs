using Aurigma.DirectMail.Sample.DomainEntities.Entities.Category;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Storages;

public interface ICategoryRepository
{
    Task<Category> GetCategoryByIdAsync(Guid id);
    Task<Category> GetCategoryByIdAsReadOnlyAsync(Guid id);

    Task<List<Category>> GetAllAsync();
    Task<List<Category>> GetAllAsReadOnlyAsync();

    Task<Category> CreateCategoryAsync(Category category);
    Task<Category> UpdateCategoryAsync(Category category);
    Task<Category> DeleteCategoryAsync(Guid id);
}
