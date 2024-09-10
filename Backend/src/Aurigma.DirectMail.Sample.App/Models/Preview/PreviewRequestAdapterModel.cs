namespace Aurigma.DirectMail.Sample.App.Models.Preview;

public class PreviewRequestAdapterModel
{
    public string DesignId { get; set; }

    public string MockupId { get; set; }

    public string UserId { get; set; }
    public DesignAtomsApi.ProductPreviewRenderingConfig Config { get; set; }

    public IEnumerable<DesignAtomsApi.VariableInfo> Variables { get; set; }
}
