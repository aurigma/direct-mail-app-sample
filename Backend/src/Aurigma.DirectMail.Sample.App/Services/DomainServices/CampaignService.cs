using Aurigma.DirectMail.Sample.App.Exceptions.Campaign;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Storages;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Campaign;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.RecipientList;
using Aurigma.DirectMail.Sample.DomainEntities.Enums;
using AutoMapper;

namespace Aurigma.DirectMail.Sample.App.Services.DomainServices;

public class CampaignService(
    ICampaignRepository repository,
    ICompanyService companyService,
    IRecipientListService recipientListService,
    IMapper mapper
) : ICampaignService
{
    private readonly ICampaignRepository _repository = repository;
    private readonly ICompanyService _companyService = companyService;
    private readonly IRecipientListService _recipientListService = recipientListService;

    private readonly IMapper _mapper = mapper;

    public async Task<Campaign> CreateCampaignAsync(Campaign campaign)
    {
        await ValidateCompany(campaign.CompanyId);
        return await _repository.CreateCampaignAsync(campaign);
    }

    public async Task<Campaign> GetCampaignAsync(Guid id)
    {
        return await _repository.GetCampaignByIdAsReadOnlyAsync(id)
            ?? throw new CampaignNotFoundException(
                id,
                $"The campaign with identifier '{id}' was not found"
            );
    }

    public async Task<List<Campaign>> GetCampaignsAsync(CampaignFilter campaignFilter)
    {
        if (campaignFilter is null)
        {
            return await _repository.GetAllAsReadOnlyAsync();
        }

        return await _repository.GetCampaignByFilterAsync(campaignFilter);
    }

    public async Task<Campaign> UpdateCampaignAsync(Campaign campaign)
    {
        ValidateIds(campaign);
        await ValidateRecipientLists(campaign.RecipientListIds);

        var dbCampaignItem =
            await _repository.GetCampaignByIdAsReadOnlyAsync(campaign.Id)
            ?? throw new CampaignNotFoundException(
                campaign.Id,
                $"The campaign with identifier '{campaign.Id}' was not found"
            );

        _mapper.Map(campaign, dbCampaignItem);
        var updatedCampaign = await _repository.UpdateCampaignAsync(dbCampaignItem);

        return updatedCampaign;
    }

    public IEnumerable<CampaignType> GetCampaignTypes()
    {
        return Enum.GetValues<CampaignType>();
    }

    public async Task<IEnumerable<Recipient>> GetCampaignRecipientsAsync(Campaign campaign)
    {
        var campaignRecipientLists = new List<RecipientList>();
        foreach (var id in campaign.RecipientListIds)
        {
            var recipientList = await _recipientListService.GetRecipientListAsync(id);
            campaignRecipientLists.Add(recipientList);
        }

        var recipients = campaignRecipientLists.SelectMany(l => l.Recipients);
        return recipients;
    }

    private async Task ValidateCompany(Guid id)
    {
        var company = await _companyService.GetCompanyAsync(id);
    }

    private void ValidateIds(Campaign campaign)
    {
        if (campaign.Id == default)
            throw new CampaignIdException("The ID of the campaign is not specified", campaign.Id);

        if (campaign.RecipientListIds.Any(id => id == default))
        {
            throw new CampaignIdException("The ID of recipient list is not specified", campaign.Id);
        }
    }

    private async Task ValidateRecipientLists(IEnumerable<Guid> ids)
    {
        var lists = await _recipientListService.GetRecipientListsAsync();
        if (ids.Any(id => !lists.Select(list => list.Id).Contains(id)))
            throw new CampaignRecipientListException(
                $"The recipient list was not found. model={Serialize(ids)}"
            );
    }

    private static string Serialize<T>(T source)
        where T : class
    {
        return StringHelper.Serialize(source);
    }
}

internal class CampaignServiceMapperProfile : Profile
{
    public CampaignServiceMapperProfile()
    {
        CreateMap<Campaign, Campaign>()
            .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
            .ForMember(dest => dest.LineItems, opt => opt.Ignore());
    }
}
