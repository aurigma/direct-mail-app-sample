using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Models.Campaign;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Campaign;
using Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;

/// <summary>
/// Application service for performing operations with campaigns.
/// </summary>
public interface ICampaignAppService
{
    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="model">Model to create a campaign.</param>
    /// <returns>The created campaign.</returns>
    Task<Campaign> CreateCampaignAsync(CampaignCreationModel model);

    /// <summary>
    /// Returns a campaign by id.
    /// </summary>
    /// <param name="id">ID of the campaign to search for.</param>
    /// <returns>The found campaign</returns>
    /// <exception cref="NotFoundAppException"> The campaign was not found.</exception>
    Task<Campaign> GetCampaignById(Guid id);

    /// <summary>
    /// Return all campaigns by filter.
    /// </summary>
    /// <param name="model">Request filter model.</param>
    /// <returns>All campaigns satisfying the filter.</returns>
    Task<IEnumerable<Campaign>> GetCampaignsAsync(CampaignRequestModel model);

    /// <summary>
    /// Updates a campaign.
    /// </summary>
    /// <param name="model"> Model to update a campaign.</param>
    /// <returns> The updated campaign.</returns>
    /// <exception cref="NotFoundAppException"> The campaign was not found.</exception>
    /// <exception cref="InvalidValueAppException"> The property of model has an invalid value.</exception>
    Task<Campaign> UpdateCampaignAsync(CampaignUpdateModel model);

    /// <summary>
    /// Returns all campaign types.
    /// </summary>
    /// <returns>All campaign types.</returns>
    IEnumerable<CampaignType> GetCampaignTypes();
}
