using System;
using Aurigma.DirectMail.Sample.WebApi.Enums;

namespace Aurigma.DirectMail.Sample.WebApi.Models.Campaign;

public class CampaignRequestModel
{
    /// <summary>
    /// Campaign type.
    /// </summary>
    public CampaignType? Type { get; set; }

    /// <summary>
    /// Linked company id.
    /// </summary>
    public Guid? CompanyId { get; set; }
}
