namespace Aurigma.DirectMail.Sample.App.Exceptions.RecipientList;

public class RecipientListNotFoundException : Exception
{
    public Guid Id { get; set; }

    public RecipientListNotFoundException() { }

    public RecipientListNotFoundException(Guid id)
    {
        Id = id;
    }

    public RecipientListNotFoundException(Guid id, string message)
        : base(message)
    {
        Id = id;
    }

    public RecipientListNotFoundException(Guid id, string message, Exception innerException)
        : base(message, innerException)
    {
        Id = id;
    }
}
