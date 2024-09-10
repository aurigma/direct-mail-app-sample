namespace Aurigma.DirectMail.Sample.App.Models.LineItem;

public class LineItemCreationModel
{
    public Guid CampaignId { get; set; }

    public int Quantity { get; set; }

    public Guid ProductId { get; set; }

    public string TemplateId { get; set; }
}
