namespace Aurigma.DirectMail.Sample.App.Models.Preview;

public class DesignPreviewToResourceRequestAdapterModel
{
    public string DesignId { get; set; }

    public int SurfaceIndex { get; set; }

    public string ResourceName { get; set; }

    public string MockupId { get; set; }

    public string SourceId { get; set; }

    public string Namespace { get; set; }

    public string UserId { get; set; } = null;

    public ICollection<DesignAtomsApi.VariableInfo> Variables { get; set; }
}
