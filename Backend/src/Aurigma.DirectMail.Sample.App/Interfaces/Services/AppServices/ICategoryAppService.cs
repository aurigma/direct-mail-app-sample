using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Models.Category;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Category;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;

/// <summary>
/// Application service for performing operations with categories.
/// </summary>
public interface ICategoryAppService
{
    /// <summary>
    /// Creates a new category
    /// </summary>
    /// <param name="model">Model to create a category.</param>
    /// <returns>The created category.</returns>
    Task<Category> CreateCategoryAsync(CategoryCreationModel model);

    /// <summary>
    /// Returns a category by id.
    /// </summary>
    /// <param name="id">ID of the category to search for.</param>
    /// <returns>The found category.</returns>
    /// <exception cref="NotFoundAppException"> The category was not found.</exception>
    Task<Category> GetCategoryById(Guid id);

    /// <summary>
    /// Return all categories.
    /// </summary>
    /// <returns>All categories.</returns>
    Task<IEnumerable<Category>> GetCategoriesAsync();
}
