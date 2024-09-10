using Aurigma.DirectMail.Sample.App.Exceptions.Campaign;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Campaign;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Company;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.RecipientList;
using Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;

/// <summary>
/// Domain service for performing operations with campaigns.
/// </summary>
public interface ICampaignService
{
    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="campaign">Model to create a campaign.</param>
    /// <returns>The created campaign.</returns>
    Task<Campaign> CreateCampaignAsync(Campaign campaign);

    /// <summary>
    /// Returns a campaign by id.
    /// </summary>
    /// <param name="id">ID of the campaign to search for.</param>
    /// <returns>The found campaign</returns>
    /// <exception cref="CampaignNotFoundException"> The campaign was not found.</exception>
    Task<Campaign> GetCampaignAsync(Guid id);

    /// <summary>
    /// Return all campaigns by filter.
    /// </summary>
    /// <param name="campaignFilter">Request filter model.</param>
    /// <returns>All campaigns satisfying the filter.</returns>
    Task<List<Campaign>> GetCampaignsAsync(CampaignFilter campaignFilter);

    /// <summary>
    /// Updates a campaign.
    /// </summary>
    /// <param name="campaign"> Model to update a campaign.</param>
    /// <returns> The updated campaign.</returns>
    /// <exception cref="CampaignNotFoundException"> The campaign was not found.</exception>
    /// <exception cref="CampaignIdException"> The IDs has an invalid value..</exception>
    /// <exception cref="CampaignRecipientListException"> The campaign's recipient list ids has an invalid value.</exception>
    Task<Campaign> UpdateCampaignAsync(Campaign campaign);

    /// <summary>
    /// Returns all campaign types.
    /// </summary>
    /// <returns>All campaign types.</returns>
    IEnumerable<CampaignType> GetCampaignTypes();

    /// <summary>
    /// Returns a campaign's recipients.
    /// </summary>
    /// <param name="campaign">Model to request recipients.</param>
    /// <returns>Campaign's recipients</returns>
    Task<IEnumerable<Recipient>> GetCampaignRecipientsAsync(Campaign campaign);
}
