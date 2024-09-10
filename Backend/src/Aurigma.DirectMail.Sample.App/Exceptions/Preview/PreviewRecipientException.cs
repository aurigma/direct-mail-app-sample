using Aurigma.DirectMail.Sample.DomainEntities.Entities.RecipientList;

namespace Aurigma.DirectMail.Sample.App.Exceptions.Preview;

public class PreviewRecipientException : Exception
{
    public Recipient Recipient { get; }

    public PreviewRecipientException(string message, Recipient recipient = null)
        : base(message)
    {
        Recipient = recipient;
    }
}
