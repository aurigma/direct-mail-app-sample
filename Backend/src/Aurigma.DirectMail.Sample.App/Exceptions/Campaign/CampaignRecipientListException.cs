namespace Aurigma.DirectMail.Sample.App.Exceptions.Campaign;

public class CampaignRecipientListException : Exception
{
    public DomainEntities.Entities.RecipientList.RecipientList RecipientList { get; }

    public CampaignRecipientListException(
        string message,
        DomainEntities.Entities.RecipientList.RecipientList recipientList = null
    )
        : base(message)
    {
        RecipientList = recipientList;
    }
}
