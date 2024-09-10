using System;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.LineItem;

public class LineItem
{
    public Guid Id { get; set; }

    public int Quantity { get; set; }

    public Guid CampaignId { get; set; }

    public Guid ProductId { get; set; }

    public string TemplateId { get; set; }

    public string DesignId { get; set; }

    public int? ProductVariantId { get; set; }
}
