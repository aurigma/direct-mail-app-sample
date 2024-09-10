using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;
using Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Category;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Product;
using AutoMapper;
using DomainStorage = Aurigma.DirectMail.Sample.App.Interfaces.Storages;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Adapters.Repositories;

public class CategoryRepositoryAdapter(ICategoryRepository repository, IMapper mapper)
    : DomainStorage.ICategoryRepository
{
    private readonly ICategoryRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        return _mapper.Map<Category>(
            await _repository.CreateCategoryAsync(_mapper.Map<CategoryDal>(category))
        );
    }

    public async Task<Category> DeleteCategoryAsync(Guid id)
    {
        return _mapper.Map<Category>(await _repository.DeleteCategoryAsync(id));
    }

    public async Task<List<Category>> GetAllAsReadOnlyAsync()
    {
        return _mapper.Map<List<Category>>(await _repository.GetAllAsReadOnlyAsync());
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return _mapper.Map<List<Category>>(await _repository.GetAllAsync());
    }

    public async Task<Category> GetCategoryByIdAsReadOnlyAsync(Guid id)
    {
        return _mapper.Map<Category>(await _repository.GetCategoryByIdAsReadOnlyAsync(id));
    }

    public async Task<Category> GetCategoryByIdAsync(Guid id)
    {
        return _mapper.Map<Category>(await _repository.GetCategoryByIdAsync(id));
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        return _mapper.Map<Category>(
            await _repository.UpdateCategoryAsync(_mapper.Map<CategoryDal>(category))
        );
    }
}

internal class CategoryRepositoryAdapterMapperProfile : Profile
{
    public CategoryRepositoryAdapterMapperProfile()
    {
        CreateMap<Product, ProductDal>()
            .ForMember(dest => dest.LastModificationTime, opt => opt.Ignore())
            .ForMember(dest => dest.CreationTime, opt => opt.Ignore());

        CreateMap<Category, CategoryDal>()
            .ForMember(dest => dest.LastModificationTime, opt => opt.Ignore())
            .ForMember(dest => dest.CreationTime, opt => opt.Ignore());

        CreateMap<ProductDal, Product>();
        CreateMap<CategoryDal, Category>();
    }
}
