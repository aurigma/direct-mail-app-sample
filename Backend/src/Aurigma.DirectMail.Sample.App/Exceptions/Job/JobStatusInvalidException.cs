using Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.App.Exceptions.Job;

public class JobStatusInvalidException : Exception
{
    public Guid Id { get; set; }
    public JobStatus Status { get; set; }

    public JobStatusInvalidException() { }

    public JobStatusInvalidException(Guid id)
    {
        Id = id;
    }

    public JobStatusInvalidException(Guid id, JobStatus jobStatus, string message)
        : base(message)
    {
        Id = id;
        Status = jobStatus;
    }

    public JobStatusInvalidException(
        Guid id,
        JobStatus jobStatus,
        string message,
        Exception innerException
    )
        : base(message, innerException)
    {
        Id = id;
        Status = jobStatus;
    }
}
