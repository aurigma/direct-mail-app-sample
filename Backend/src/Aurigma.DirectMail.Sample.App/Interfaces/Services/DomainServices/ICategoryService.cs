using Aurigma.DirectMail.Sample.App.Exceptions.Category;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Category;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;

/// <summary>
/// Domain service for performing operations with categories.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Creates a new category
    /// </summary>
    /// <param name="category">Model to create a category.</param>
    /// <returns>The created category.</returns>
    Task<Category> CreateCategoryAsync(Category category);

    /// <summary>
    /// Returns a category by id.
    /// </summary>
    /// <param name="id">ID of the category to search for.</param>
    /// <returns>The found category.</returns>
    /// <exception cref="CategoryNotFoundException"> The category was not found.</exception>
    Task<Category> GetCategoryAsync(Guid id);

    /// <summary>
    /// Return all categories.
    /// </summary>
    /// <returns>All categories.</returns>
    Task<List<Category>> GetCategoriesAsync();
}
