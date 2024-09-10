using System;
using System.Collections.Generic;
using Aurigma.DirectMail.Sample.WebApi.Dtos.LineItem;
using Aurigma.DirectMail.Sample.WebApi.Enums;

namespace Aurigma.DirectMail.Sample.WebApi.Dtos.Campaign;

public class CampaignDto
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
    ///
    /// </summary>
    public IEnumerable<LineItemDto> LineItems { get; set; }

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<Guid> RecipientListIds { get; set; }
}
