using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;
using Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Category;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Product;
using AutoMapper;
using DomainStorage = Aurigma.DirectMail.Sample.App.Interfaces.Storages;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Adapters.Repositories;

public class ProductRepositoryAdapter(IProductRepository productRepository, IMapper mapper)
    : DomainStorage.IProductRepository
{
    private readonly IProductRepository _repository = productRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Product> CreateProductAsync(Product product)
    {
        return _mapper.Map<Product>(
            await _repository.CreateProductAsync(_mapper.Map<ProductDal>(product))
        );
    }

    public async Task<Product> DeleteProductAsync(Guid id)
    {
        return _mapper.Map<Product>(await _repository.DeleteProductAsync(id));
    }

    public async Task<List<Product>> GetAllAsReadOnlyAsync()
    {
        return _mapper.Map<List<Product>>(await _repository.GetAllAsReadOnlyAsync());
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return _mapper.Map<List<Product>>(await _repository.GetAllAsync());
    }

    public async Task<Product> GetProductByIdAsReadOnlyAsync(Guid id)
    {
        return _mapper.Map<Product>(await _repository.GetProductByIdAsReadOnlyAsync(id));
    }

    public async Task<Product> GetProductByIdAsync(Guid id)
    {
        return _mapper.Map<Product>(await _repository.GetProductByIdAsync(id));
    }

    public async Task<List<Product>> GetProductsByFilterAsync(ProductFilter filter)
    {
        return _mapper.Map<List<Product>>(await _repository.GetProductsByFilterAsync(filter));
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        return _mapper.Map<Product>(
            await _repository.UpdateProductAsync(_mapper.Map<ProductDal>(product))
        );
    }
}

internal class ProductRepositoryAdapterMapperProfile : Profile
{
    public ProductRepositoryAdapterMapperProfile()
    {
        CreateMap<Category, CategoryDal>()
            .ForMember(dest => dest.LastModificationTime, opt => opt.Ignore())
            .ForMember(dest => dest.CreationTime, opt => opt.Ignore());

        CreateMap<Product, ProductDal>()
            .ForMember(dest => dest.LastModificationTime, opt => opt.Ignore())
            .ForMember(dest => dest.CreationTime, opt => opt.Ignore());

        CreateMap<CategoryDal, Category>();
        CreateMap<ProductDal, Product>();
    }
}
