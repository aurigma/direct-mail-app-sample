using System;
using System.Collections.Generic;
using Aurigma.DirectMail.Sample.WebApi.Dtos.LineItem;
using Aurigma.DirectMail.Sample.WebApi.Enums;

namespace Aurigma.DirectMail.Sample.WebApi.Models.Campaign;

public class CampaignUpdateModel
{
    /// <summary>
    /// Title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Campaign type.
    /// </summary>
    public CampaignType Type { get; set; }

    /// <summary>
    /// Collection of recipient list ids.
    /// </summary>
    public IEnumerable<Guid> RecipientListIds { get; set; }
}
