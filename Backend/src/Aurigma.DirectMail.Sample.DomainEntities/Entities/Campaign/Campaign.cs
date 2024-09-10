using System;
using System.Collections.Generic;
using Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.Campaign;

public class Campaign
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Campaign type.
    /// </summary>
    public CampaignType Type { get; set; }

    /// <summary>
    /// Company id.
    /// </summary>
    public Guid CompanyId { get; set; }

    /// <summary>
    /// Linked line items.
    /// </summary>
    public IEnumerable<LineItem.LineItem> LineItems { get; set; }

    /// <summary>
    /// Linked recipient list ids.
    /// </summary>
    public IEnumerable<Guid> RecipientListIds { get; set; } = new List<Guid>();
}
