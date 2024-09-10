using System.Collections.Generic;

namespace Aurigma.DirectMail.Sample.WebApi.Dtos.IntegratedProduct;

public class IntegratedProductTemplateDetailsDto
{
    public string TemplateId { get; set; }
    public string TemplateName { get; set; }

    public int ProductVariantId { get; set; }

    public IEnumerable<IntegratedProductTemplateDetailsOptionDto> Options { get; set; }

    public IEnumerable<string> PreviewUrls { get; set; }

    public IDictionary<string, object> CustomFields { get; set; }
}
