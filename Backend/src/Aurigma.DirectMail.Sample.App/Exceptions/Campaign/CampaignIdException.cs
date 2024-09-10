namespace Aurigma.DirectMail.Sample.App.Exceptions.Campaign;

public class CampaignIdException : Exception
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public Guid Id { get; }

    public CampaignIdException(string message, Guid id)
        : base(message)
    {
        Id = id;
    }
}
