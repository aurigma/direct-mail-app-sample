using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Campaign;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Storages;

public interface ICampaignRepository
{
    Task<Campaign> GetCampaignByIdAsync(Guid id);
    Task<Campaign> GetCampaignByIdAsReadOnlyAsync(Guid id);

    Task<List<Campaign>> GetAllAsync();
    Task<List<Campaign>> GetAllAsReadOnlyAsync();
    Task<List<Campaign>> GetCampaignByFilterAsync(CampaignFilter filter);

    Task<Campaign> CreateCampaignAsync(Campaign campaign);
    Task<Campaign> UpdateCampaignAsync(Campaign campaign);
    Task<Campaign> DeleteCampaignAsync(Guid id);
}
