namespace Aurigma.DirectMail.Sample.App.Models.IntegratedProduct;

public class IntegratedProductResourcesRequestAppModel
{
    public Guid ProductId { get; set; }
    public int ProductVariantId { get; set; }

    public string TemplateId { get; set; }

    public string UserId { get; set; }
}
