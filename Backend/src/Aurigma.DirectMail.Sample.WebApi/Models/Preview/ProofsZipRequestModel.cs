using System;

namespace Aurigma.DirectMail.Sample.WebApi.Models.Preview;

public class ProofsZipRequestModel
{
    /// <summary>
    /// Current line item id.
    /// </summary>
    public Guid LineItemId { get; set; }

    /// <summary>
    /// Rendering config.
    /// </summary>
    public ProofsZipRequestConfigModel Config { get; set; }
}
