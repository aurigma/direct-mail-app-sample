using Aurigma.DirectMail.Sample.DomainEntities.Entities.LineItem;

namespace Aurigma.DirectMail.Sample.App.Filters;

/// <summary>
/// Filter for <see cref="LineItem"/>
/// </summary>
public class LineItemFilter
{
    /// <summary>
    /// Line items with this product id.
    /// </summary>
    public Guid? ProductId { get; set; }

    /// <summary>
    /// Line items with this campaign id.
    /// </summary>
    public Guid? CampaignId { get; set; }
}
