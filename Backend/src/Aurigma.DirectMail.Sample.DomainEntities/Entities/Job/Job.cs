using System;
using Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.Job;

public class Job
{
    public Guid Id { get; set; }

    public int CustomersCanvasProjectId { get; set; }

    public JobStatus Status { get; set; }

    public Guid LineItemId { get; set; }
}
