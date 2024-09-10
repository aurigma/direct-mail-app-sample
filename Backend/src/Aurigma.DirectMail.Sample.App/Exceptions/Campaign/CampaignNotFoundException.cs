namespace Aurigma.DirectMail.Sample.App.Exceptions.Campaign;

public class CampaignNotFoundException : Exception
{
    public Guid Id { get; set; }

    public CampaignNotFoundException() { }

    public CampaignNotFoundException(Guid id)
    {
        Id = id;
    }

    public CampaignNotFoundException(Guid id, string message)
        : base(message)
    {
        Id = id;
    }

    public CampaignNotFoundException(Guid id, string message, Exception innerException)
        : base(message, innerException)
    {
        Id = id;
    }
}
