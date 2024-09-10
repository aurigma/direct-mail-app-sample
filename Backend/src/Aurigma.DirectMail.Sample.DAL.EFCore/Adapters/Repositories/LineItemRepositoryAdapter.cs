using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;
using Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Campaign;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.LineItem;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Product;
using AutoMapper;
using DomainStorage = Aurigma.DirectMail.Sample.App.Interfaces.Storages;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Adapters.Repositories;

public class LineItemRepositoryAdapter(ILineItemRepository repository, IMapper mapper)
    : DomainStorage.ILineItemRepository
{
    private ILineItemRepository _repository = repository;
    private IMapper _mapper = mapper;

    public async Task<LineItem> CreateLineItemAsync(LineItem lineItem)
    {
        return _mapper.Map<LineItem>(
            await _repository.CreateLineItemAsync(_mapper.Map<LineItemDal>(lineItem))
        );
    }

    public async Task<LineItem> DeleteLineItemAsync(Guid id)
    {
        return _mapper.Map<LineItem>(await _repository.DeleteLineItemAsync(id));
    }

    public async Task<List<LineItem>> GetAllAsReadOnlyAsync()
    {
        return _mapper.Map<List<LineItem>>(await _repository.GetAllAsReadOnlyAsync());
    }

    public async Task<List<LineItem>> GetAllAsync()
    {
        return _mapper.Map<List<LineItem>>(await _repository.GetAllAsync());
    }

    public async Task<LineItem> GetLineItemByIdAsReadOnlyAsync(Guid id)
    {
        return _mapper.Map<LineItem>(await _repository.GetLineItemByIdAsReadOnlyAsync(id));
    }

    public async Task<LineItem> GetLineItemByIdAsync(Guid id)
    {
        return _mapper.Map<LineItem>(await _repository.GetLineItemByIdAsync(id));
    }

    public async Task<List<LineItem>> GetLineItemsByFilterAsync(LineItemFilter filter)
    {
        return _mapper.Map<List<LineItem>>(await _repository.GetLineItemsByFilterAsync(filter));
    }

    public async Task<LineItem> UpdateLineItemAsync(LineItem lineItem)
    {
        return _mapper.Map<LineItem>(
            await _repository.UpdateLineItemAsync(_mapper.Map<LineItemDal>(lineItem))
        );
    }
}

internal class LineItemRepositoryAdapterMapperProfile : Profile
{
    public LineItemRepositoryAdapterMapperProfile()
    {
        CreateMap<Campaign, CampaignDal>()
            .ForMember(dest => dest.LastModificationTime, opt => opt.Ignore())
            .ForMember(dest => dest.CreationTime, opt => opt.Ignore());

        CreateMap<Product, ProductDal>()
            .ForMember(dest => dest.LastModificationTime, opt => opt.Ignore())
            .ForMember(dest => dest.CreationTime, opt => opt.Ignore())
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

        CreateMap<LineItem, LineItemDal>()
            .ForMember(dest => dest.LastModificationTime, opt => opt.Ignore())
            .ForMember(dest => dest.CreationTime, opt => opt.Ignore());

        CreateMap<CampaignDal, Campaign>();
        CreateMap<ProductDal, Product>();
        CreateMap<LineItemDal, LineItem>();
    }
}
