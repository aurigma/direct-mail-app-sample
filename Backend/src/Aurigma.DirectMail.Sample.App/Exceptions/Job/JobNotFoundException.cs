namespace Aurigma.DirectMail.Sample.App.Exceptions.Job;

public class JobNotFoundException : Exception
{
    public Guid Id { get; set; }

    public JobNotFoundException() { }

    public JobNotFoundException(Guid id)
    {
        Id = id;
    }

    public JobNotFoundException(Guid id, string message)
        : base(message)
    {
        Id = id;
    }

    public JobNotFoundException(Guid id, string message, Exception innerException)
        : base(message, innerException)
    {
        Id = id;
    }
}
