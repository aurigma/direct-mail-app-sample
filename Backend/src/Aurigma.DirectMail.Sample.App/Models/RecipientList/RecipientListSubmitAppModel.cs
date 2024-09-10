namespace Aurigma.DirectMail.Sample.App.Models.RecipientList;

public class RecipientListSubmitAppModel
{
    public Guid CampaignId { get; set; }

    public IEnumerable<Guid> RecipientListIds { get; set; }
}
