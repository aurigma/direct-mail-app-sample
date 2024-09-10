using System;
using Aurigma.DirectMail.Sample.WebApi.Enums;

namespace Aurigma.DirectMail.Sample.WebApi.Models.Campaign;

public class CampaignCreationModel
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
    /// Company Id.
    /// </summary>
    public Guid CompanyId { get; set; }
}
