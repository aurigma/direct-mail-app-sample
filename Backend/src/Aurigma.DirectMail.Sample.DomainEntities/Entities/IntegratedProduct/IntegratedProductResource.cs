using Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.IntegratedProduct;

public class IntegratedProductResource
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }

    public IntegratedProductResourceType Type { get; set; }

    public IntegratedProductResourcePreview Preview { get; set; }
}
