using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;
using Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Campaign;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.LineItem;
using AutoMapper;
using DomainStorage = Aurigma.DirectMail.Sample.App.Interfaces.Storages;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Adapters.Repositories;

public class CampaignRepositoryAdapter(ICampaignRepository repository, IMapper mapper)
    : DomainStorage.ICampaignRepository
{
    private readonly ICampaignRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<Campaign> CreateCampaignAsync(Campaign campaign)
    {
        return _mapper.Map<Campaign>(
            await _repository.CreateCampaignAsync(_mapper.Map<CampaignDal>(campaign))
        );
    }

    public async Task<Campaign> DeleteCampaignAsync(Guid id)
    {
        return _mapper.Map<Campaign>(await _repository.DeleteCampaignAsync(id));
    }

    public async Task<List<Campaign>> GetAllAsReadOnlyAsync()
    {
        var campaingsDal = await _repository.GetAllAsReadOnlyAsync();
        var campaigns = campaingsDal
            .Select(campaign => new Campaign
            {
                Title = campaign.Title,
                CompanyId = campaign.CompanyId,
                Id = campaign.Id,
                Type = campaign.Type,
                RecipientListIds = campaign
                    .RecipientListCampaigns.Select(list => list.RecipientListId)
                    .ToList(),
                LineItems = _mapper.Map<IEnumerable<LineItem>>(campaign.LineItems),
            })
            .ToList();

        return campaigns;
    }

    public async Task<List<Campaign>> GetAllAsync()
    {
        return _mapper.Map<List<Campaign>>(await _repository.GetAllAsync());
    }

    public async Task<List<Campaign>> GetCampaignByFilterAsync(CampaignFilter filter)
    {
        var campaingsDal = await _repository.GetCampaignByFilterAsync(filter);
        var campaigns = campaingsDal
            .Select(campaign => new Campaign
            {
                Title = campaign.Title,
                CompanyId = campaign.CompanyId,
                Id = campaign.Id,
                Type = campaign.Type,
                RecipientListIds = campaign
                    .RecipientListCampaigns.Select(list => list.RecipientListId)
                    .ToList(),
                LineItems = _mapper.Map<IEnumerable<LineItem>>(campaign.LineItems),
            })
            .ToList();

        return campaigns;
    }

    public async Task<Campaign> GetCampaignByIdAsReadOnlyAsync(Guid id)
    {
        var campaignDal = await _repository.GetCampaignByIdAsReadOnlyAsync(id);
        if (campaignDal == null)
            return null;
        var campaign = new Campaign
        {
            Title = campaignDal.Title,
            CompanyId = campaignDal.CompanyId,
            Id = campaignDal.Id,
            Type = campaignDal.Type,
            RecipientListIds = campaignDal
                .RecipientListCampaigns.Select(list => list.RecipientListId)
                .ToList(),
            LineItems = _mapper.Map<IEnumerable<LineItem>>(campaignDal.LineItems),
        };
        return campaign;
    }

    public async Task<Campaign> GetCampaignByIdAsync(Guid id)
    {
        return _mapper.Map<Campaign>(await _repository.GetCampaignByIdAsync(id));
    }

    public async Task<Campaign> UpdateCampaignAsync(Campaign campaign)
    {
        var campaignDal = _mapper.Map<CampaignDal>(campaign);
        campaignDal.RecipientListCampaigns = campaign
            .RecipientListIds.Select(p => new RecipientListCampaignDal
            {
                CampaignId = campaign.Id,
                RecipientListId = p,
            })
            .ToList();

        var updatedDalEntity = await _repository.UpdateCampaignAsync(campaignDal);
        var updatedDomainEntity = _mapper.Map<Campaign>(updatedDalEntity);
        updatedDomainEntity.RecipientListIds = updatedDalEntity
            .RecipientListCampaigns.Select(l => l.RecipientListId)
            .ToList();

        return updatedDomainEntity;
    }
}

internal class CampaignRepositoryAdapterMapperProfile : Profile
{
    public CampaignRepositoryAdapterMapperProfile()
    {
        CreateMap<LineItem, LineItemDal>()
            .ForMember(dest => dest.LastModificationTime, opt => opt.Ignore())
            .ForMember(dest => dest.CreationTime, opt => opt.Ignore());

        CreateMap<Campaign, CampaignDal>()
            .ForMember(dest => dest.LastModificationTime, opt => opt.Ignore())
            .ForMember(dest => dest.CreationTime, opt => opt.Ignore());

        CreateMap<LineItemDal, LineItem>();
        CreateMap<CampaignDal, Campaign>();
    }
}
