using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;
using Aurigma.DirectMail.Sample.DAL.EFCore.Extensions;
using Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Repositories;

public class ProductRepository : BaseRepository, IProductRepository
{
    private readonly DbSet<ProductDal> _products;
    private readonly IMapper _mapper;

    public ProductRepository(DbContext context, IMapper mapper)
        : base(context)
    {
        _products = DataContext.Set<ProductDal>();
        _mapper = mapper;
    }

    public async Task<ProductDal> CreateProductAsync(ProductDal company)
    {
        PrepareForCreation(company);
        var createdEntity = (await _products.AddAsync(company)).Entity;
        await DataContext.SaveChangesAsync();

        return createdEntity;
    }

    public async Task<ProductDal> DeleteProductAsync(Guid id)
    {
        var deletingEntity = await _products.FirstOrDefaultAsync(x => x.Id == id);
        if (deletingEntity == null)
            return null;

        _products.Remove(deletingEntity);
        await DataContext.SaveChangesAsync();

        return deletingEntity;
    }

    public Task<IEnumerable<ProductDal>> GetAllAsReadOnlyAsync()
    {
        return Task.FromResult<IEnumerable<ProductDal>>(
            _products.Include(p => p.Category).AsNoTracking()
        );
    }

    public Task<IEnumerable<ProductDal>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<ProductDal>>(_products.Include(p => p.Category));
    }

    public async Task<ProductDal> GetProductByIdAsReadOnlyAsync(Guid id)
    {
        return await _products
            .AsNoTracking()
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<ProductDal> GetProductByIdAsync(Guid id)
    {
        return await _products.Include(p => p.Category).FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<IEnumerable<ProductDal>> GetProductsByFilterAsync(ProductFilter filter)
    {
        return Task.FromResult<IEnumerable<ProductDal>>(
            _products.Include(p => p.Category).Where(filter.GetPredicate()).AsQueryable()
        );
    }

    public async Task<ProductDal> UpdateProductAsync(ProductDal product)
    {
        var updatingEntity = await _products.FirstOrDefaultAsync(x => x.Id == product.Id);
        if (updatingEntity == null)
            return null;

        _mapper.Map(product, updatingEntity);
        PrepareForUpdate(product);
        await DataContext.SaveChangesAsync();

        return updatingEntity;
    }
}

internal class ProductRepositoryMapperProfile : Profile
{
    public ProductRepositoryMapperProfile()
    {
        CreateMap<ProductDal, ProductDal>();
    }
}
