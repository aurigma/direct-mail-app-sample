using System;

namespace Aurigma.DirectMail.Sample.WebApi.Models.Preview;

public class PreviewRequestModel
{
    /// <summary>
    /// Recipient id.
    /// </summary>
    public Guid? RecipientId { get; set; }

    /// <summary>
    /// Current line item id.
    /// </summary>
    public Guid LineItemId { get; set; }

    /// <summary>
    /// Rendering config.
    /// </summary>
    public PreviewConfigRequestModel Config { get; set; }
}
