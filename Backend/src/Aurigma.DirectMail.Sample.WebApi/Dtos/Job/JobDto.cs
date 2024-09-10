using System;
using Aurigma.DirectMail.Sample.WebApi.Enums;

namespace Aurigma.DirectMail.Sample.WebApi.Dtos.Job;

public class JobDto
{
    /// <summary>
    /// Job id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Customer's Canvas project id.
    /// </summary>
    public int CustomersCanvasProjectId { get; set; }

    /// <summary>
    /// Job processing status.
    /// </summary>
    public JobStatus Status { get; set; }
}
