using DomainCampaign = Aurigma.DirectMail.Sample.DomainEntities.Entities.Campaign;
using DomainCompany = Aurigma.DirectMail.Sample.DomainEntities.Entities.Company;
using DomainLineItem = Aurigma.DirectMail.Sample.DomainEntities.Entities.LineItem;

namespace Aurigma.DirectMail.Sample.App.Models.Project;

public class ProjectCreationAppModel
{
    public DomainEntities.Entities.LineItem.LineItem LineItem { get; set; }
    public DomainEntities.Entities.Campaign.Campaign Campaign { get; set; }

    public DomainEntities.Entities.Company.Company Company { get; set; }
    public string DesignId { get; set; }

    public IDictionary<string, object> Fields { get; set; }
}
