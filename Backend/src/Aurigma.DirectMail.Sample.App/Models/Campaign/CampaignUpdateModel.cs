using Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.App.Models.Campaign;

public class CampaignUpdateModel
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Campaign type.
    /// </summary>
    public CampaignType Type { get; set; }

    /// <summary>
    /// Collection of recipient list ids.
    /// </summary>
    public IEnumerable<Guid> RecipientListIds { get; set; }
}
