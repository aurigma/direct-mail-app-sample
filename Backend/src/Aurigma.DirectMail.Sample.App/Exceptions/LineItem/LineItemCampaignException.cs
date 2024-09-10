namespace Aurigma.DirectMail.Sample.App.Exceptions.LineItem;

public class LineItemCampaignException : Exception
{
    public Guid Id { get; }

    public LineItemCampaignException(Guid campaignId, string message)
        : base(message)
    {
        Id = campaignId;
    }

    public LineItemCampaignException(Guid campaignId, string message, Exception innerException)
        : base(message, innerException)
    {
        Id = campaignId;
    }
}
