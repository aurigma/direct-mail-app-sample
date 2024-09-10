namespace Aurigma.DirectMail.Sample.App.Models.Preview;

public class ProofRequestAdapterModel
{
    public string DesignId { get; set; }

    public string UserId { get; set; }

    public IEnumerable<DesignAtomsApi.VariableInfo> Variables { get; set; }

    public DesignAtomsApi.ProductProofRenderingConfig Config { get; set; }
}
