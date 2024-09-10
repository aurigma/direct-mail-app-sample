using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Exceptions.Campaign;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Models.Campaign;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Campaign;
using Aurigma.DirectMail.Sample.DomainEntities.Enums;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aurigma.DirectMail.Sample.App.Services.AppServices;

public class CampaignAppService(
    ILogger<CampaignAppService> logger,
    ICampaignService campaignService,
    IMapper mapper
) : ICampaignAppService
{
    private readonly ILogger<CampaignAppService> _logger = logger;
    private readonly ICampaignService _campaignService = campaignService;
    private readonly IMapper _mapper = mapper;

    public async Task<Campaign> CreateCampaignAsync(CampaignCreationModel model)
    {
        try
        {
            var createdCampaign = await _campaignService.CreateCampaignAsync(
                _mapper.Map<Campaign>(model)
            );
            LogSuccessCreation(createdCampaign);
            return createdCampaign;
        }
        catch (Exception ex)
        {
            LogErrorCreation(ex, model);
            throw;
        }
    }

    public async Task<IEnumerable<Campaign>> GetCampaignsAsync(CampaignRequestModel model)
    {
        try
        {
            var campaigns = await _campaignService.GetCampaignsAsync(
                _mapper.Map<CampaignFilter>(model)
            );
            LogSuccessGetting(campaigns.Count, model);
            return campaigns;
        }
        catch (Exception ex)
        {
            LogErrorGetting(ex, model);
            throw;
        }
    }

    public async Task<Campaign> GetCampaignById(Guid id)
    {
        try
        {
            var campaign = await _campaignService.GetCampaignAsync(id);
            LogSuccessGettingById(campaign);
            return campaign;
        }
        catch (CampaignNotFoundException ex)
        {
            LogGettingNotFound(ex, id);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorGettingById(ex, id);
            throw;
        }
    }

    public async Task<Campaign> UpdateCampaignAsync(CampaignUpdateModel model)
    {
        try
        {
            var campaign = await _campaignService.UpdateCampaignAsync(_mapper.Map<Campaign>(model));
            LogAnUpdate(campaign);
            return campaign;
        }
        catch (CampaignNotFoundException ex)
        {
            LogAnUpdateNotFound(ex, model.Id);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (CampaignIdException ex)
        {
            LogAnUpdateInvalid(ex, model);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (CampaignRecipientListException ex)
        {
            LogAnUpdateInvalid(ex, model);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogAnUpdateError(ex, model);
            throw;
        }
    }

    public IEnumerable<CampaignType> GetCampaignTypes()
    {
        try
        {
            var campaignTypes = _campaignService.GetCampaignTypes();
            LogSuccessGettingCampaignTypes(campaignTypes.Count());
            return campaignTypes;
        }
        catch (Exception ex)
        {
            LogErrorGettingCampaignTypes(ex);
            throw;
        }
    }

    #region Logging
    private void LogSuccessCreation(Campaign campaign)
    {
        _logger.LogDebug(
            $"The campaign was created. Id: {campaign.Id}, campaign={Serialize(campaign)}"
        );
    }

    private void LogErrorCreation(Exception ex, CampaignCreationModel model)
    {
        _logger.LogError(ex, $"Failed to create a campaign. Request model={Serialize(model)}");
    }

    private void LogSuccessGetting(int campaignCount, CampaignRequestModel model)
    {
        _logger.LogDebug(
            $"Campaigns was returned. Campaigns count={campaignCount}, Request model={Serialize(model)}"
        );
    }

    private void LogErrorGetting(Exception ex, CampaignRequestModel model)
    {
        _logger.LogError(ex, $"Failed to request a campaigns. Request model={Serialize(model)}");
    }

    private void LogSuccessGettingById(Campaign model)
    {
        _logger.LogDebug(
            $"Campaign was returned by ID. Entity id ={model.Id}. Campaign={Serialize(model)}"
        );
    }

    private void LogErrorGettingById(Exception ex, Guid id)
    {
        _logger.LogError(ex, $"Failed to request a campaign by ID. Entity id ={id}");
    }

    private void LogGettingNotFound(Exception ex, Guid id)
    {
        _logger.LogError(ex, $"The campaign was not found. id={id}");
    }

    private void LogSuccessGettingCampaignTypes(int campaignTypesCount)
    {
        _logger.LogDebug($"Campaign types was returned. Types count={campaignTypesCount}.");
    }

    private void LogErrorGettingCampaignTypes(Exception ex)
    {
        _logger.LogError(ex, $"Failed to request a campaign types.");
    }

    private void LogAnUpdate(Campaign campaign)
    {
        _logger.LogDebug($"The campaign was updated. campaign={Serialize(campaign)}");
    }

    private void LogAnUpdateInvalid(Exception ex, CampaignUpdateModel model)
    {
        _logger.LogWarning(ex, $"Failed to update the campaign. model={Serialize(model)}");
    }

    private void LogAnUpdateNotFound(Exception ex, Guid id)
    {
        _logger.LogWarning(ex, $"The campaign to update was not found. campaignId={id}");
    }

    private void LogAnUpdateError(Exception ex, CampaignUpdateModel model)
    {
        _logger.LogError(ex, $"Failed to update the campaign. model={Serialize(model)}");
    }

    #endregion Logging

    private static string Serialize<T>(T source)
        where T : class
    {
        return StringHelper.Serialize(source);
    }
}

internal class CampaignAppServiceMapperProfile : Profile
{
    public CampaignAppServiceMapperProfile()
    {
        CreateMap<CampaignCreationModel, Campaign>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
        CreateMap<CampaignRequestModel, CampaignFilter>();
        CreateMap<CampaignUpdateModel, Campaign>();
    }
}
