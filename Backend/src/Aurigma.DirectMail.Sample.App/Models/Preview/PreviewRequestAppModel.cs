namespace Aurigma.DirectMail.Sample.App.Models.Preview;

public class PreviewRequestAppModel
{
    public Guid? RecipientId { get; set; }

    public Guid LineItemId { get; set; }

    public PreviewRequestConfigAppModel Config { get; set; }
}
