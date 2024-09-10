using System.Collections.Generic;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.IntegratedProduct;

public class IntegratedProductTemplateDetails
{
    public string TemplateId { get; set; }
    public string TemplateName { get; set; }

    public int ProductVariantId { get; set; }

    public IEnumerable<IntegratedProductTemplateDetailsOption> Options { get; set; }

    public IEnumerable<string> PreviewUrls { get; set; }

    public IDictionary<string, object> CustomFields { get; set; }
}
