using System;

namespace Aurigma.DirectMail.Sample.WebApi.Dtos.LineItem;

public class LineItemDto
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public Guid? ProductId { get; set; }
    public Guid? CampaignId { get; set; }

    public string TemplateId { get; set; }

    public int? ProductVariantId { get; set; }

    /// <summary>
    /// Personalized design id.
    /// </summary>
    public string DesignId { get; set; }
}
