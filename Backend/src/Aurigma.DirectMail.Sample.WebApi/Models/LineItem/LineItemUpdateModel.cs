using System;

namespace Aurigma.DirectMail.Sample.WebApi.Models.LineItem;

public class LineItemUpdateModel
{
    /// <summary>
    /// Campaign id.
    /// </summary>
    public Guid CampaignId { get; set; }

    /// <summary>
    /// Product quantity.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Product id.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Template id.
    /// </summary>
    public string TemplateId { get; set; }

    /// <summary>
    /// Personalized design id.
    /// </summary>
    public string DesignId { get; set; }

    /// <summary>
    /// Selected product variant id.
    /// </summary>
    public int? ProductVariantId { get; set; }
}
