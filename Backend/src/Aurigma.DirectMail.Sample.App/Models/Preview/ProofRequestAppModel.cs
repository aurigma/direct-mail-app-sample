namespace Aurigma.DirectMail.Sample.App.Models.Preview;

public class ProofRequestAppModel
{
    public Guid RecipientId { get; set; }

    public Guid LineItemId { get; set; }

    public ProofRequestConfigAppModel Config { get; set; }
}
