using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;
using Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Repositories;

public class CategoryRepository : BaseRepository, ICategoryRepository
{
    private readonly DbSet<CategoryDal> _categories;
    private readonly IMapper _mapper;

    public CategoryRepository(DbContext context, IMapper mapper)
        : base(context)
    {
        _categories = DataContext.Set<CategoryDal>();
        _mapper = mapper;
    }

    public async Task<CategoryDal> CreateCategoryAsync(CategoryDal category)
    {
        PrepareForCreation(category);

        var createdEntity = (await _categories.AddAsync(category)).Entity;
        await DataContext.SaveChangesAsync();

        return createdEntity;
    }

    public async Task<CategoryDal> DeleteCategoryAsync(Guid id)
    {
        var deletingEntity = await _categories.FirstOrDefaultAsync(c => c.Id == id);
        if (deletingEntity == null)
            return null;

        _categories.Remove(deletingEntity);
        await DataContext.SaveChangesAsync();

        return deletingEntity;
    }

    public Task<IEnumerable<CategoryDal>> GetAllAsReadOnlyAsync()
    {
        return Task.FromResult<IEnumerable<CategoryDal>>(
            _categories.Include(c => c.Products).AsNoTracking()
        );
    }

    public Task<IEnumerable<CategoryDal>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<CategoryDal>>(_categories.Include(c => c.Products));
    }

    public async Task<CategoryDal> GetCategoryByIdAsReadOnlyAsync(Guid id)
    {
        return await _categories
            .AsNoTracking()
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<CategoryDal> GetCategoryByIdAsync(Guid id)
    {
        return await _categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<CategoryDal> UpdateCategoryAsync(CategoryDal category)
    {
        var updatingEntity = await _categories.FirstOrDefaultAsync(c => c.Id == category.Id);
        if (updatingEntity == null)
            return null;

        _mapper.Map(category, updatingEntity);
        PrepareForUpdate(category);
        await DataContext.SaveChangesAsync();

        return updatingEntity;
    }
}

internal class CategoryRepositoryMapperProfile : Profile
{
    public CategoryRepositoryMapperProfile()
    {
        CreateMap<CategoryDal, CategoryDal>();
    }
}
