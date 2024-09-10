using Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.App.Models.Campaign;

public class CampaignRequestModel
{
    public CampaignType? Type { get; set; }

    public Guid? CompanyId { get; set; }
}
