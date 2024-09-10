namespace Aurigma.DirectMail.Sample.App.Models.IntegratedProduct;

public class IntegrationProductOptionRequestModel
{
    /// <summary>
    ///
    /// </summary>
    public string TemplateTitle { get; set; }

    public IEnumerable<IntegrationProductOptionItemRequestModel> Options { get; set; }
}

public class IntegrationProductOptionItemRequestModel
{
    /// <summary>
    /// Customer's canvas product option id.
    /// </summary>
    public int OptionId { get; set; }

    /// <summary>
    /// / Customer's canvas product option value ids.
    /// </summary>
    public IEnumerable<int> ValueIds { get; set; }
}
