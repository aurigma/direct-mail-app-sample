using System.Collections.Generic;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.IntegratedProduct;

public class IntegratedProductTemplate
{
    public string TemplateId { get; set; }
    public string TemplateName { get; set; }

    public string PreviewUrl { get; set; }

    public int ProductVariantId { get; set; }
}
