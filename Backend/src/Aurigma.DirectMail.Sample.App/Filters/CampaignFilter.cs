using Aurigma.DirectMail.Sample.DomainEntities.Entities.Campaign;
using Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.App.Filters;

/// <summary>
/// Filter for <see cref="Campaign"/>
/// </summary>
public class CampaignFilter
{
    /// <summary>
    /// Campaign type.
    /// </summary>
    public CampaignType? Type { get; set; }

    /// <summary>
    /// Company id.
    /// </summary>
    public Guid? CompanyId { get; set; }
}
