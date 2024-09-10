using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;

public interface ICampaignRepository
{
    Task<CampaignDal> GetCampaignByIdAsync(Guid id);
    Task<CampaignDal> GetCampaignByIdAsReadOnlyAsync(Guid id);

    Task<IEnumerable<CampaignDal>> GetAllAsync();
    Task<IEnumerable<CampaignDal>> GetAllAsReadOnlyAsync();
    Task<IEnumerable<CampaignDal>> GetCampaignByFilterAsync(CampaignFilter filter);

    Task<CampaignDal> CreateCampaignAsync(CampaignDal campaign);
    Task<CampaignDal> UpdateCampaignAsync(CampaignDal campaign);
    Task<CampaignDal> DeleteCampaignAsync(Guid id);
}
