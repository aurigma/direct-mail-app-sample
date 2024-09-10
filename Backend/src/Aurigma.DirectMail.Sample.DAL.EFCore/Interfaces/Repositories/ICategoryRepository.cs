using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<CategoryDal> GetCategoryByIdAsync(Guid id);
    Task<CategoryDal> GetCategoryByIdAsReadOnlyAsync(Guid id);

    Task<IEnumerable<CategoryDal>> GetAllAsync();
    Task<IEnumerable<CategoryDal>> GetAllAsReadOnlyAsync();

    Task<CategoryDal> CreateCategoryAsync(CategoryDal category);
    Task<CategoryDal> UpdateCategoryAsync(CategoryDal category);
    Task<CategoryDal> DeleteCategoryAsync(Guid id);
}
