using System;

namespace Aurigma.DirectMail.Sample.WebApi.Models.Preview;

public class ProofRequestModel
{
    /// <summary>
    /// Recipient id.
    /// </summary>
    public Guid RecipientId { get; set; }

    /// <summary>
    /// Current line item id.
    /// </summary>
    public Guid LineItemId { get; set; }

    /// <summary>
    /// Rendering parameters.
    /// </summary>
    public ProofRequestConfigModel Config { get; set; }
}
