namespace Aurigma.DirectMail.Sample.App.Exceptions.Preview;

public class PreviewLineItemException : Exception
{
    public Guid Id { get; set; }

    public PreviewLineItemException() { }

    public PreviewLineItemException(Guid id)
    {
        Id = id;
    }

    public PreviewLineItemException(Guid id, string message)
        : base(message)
    {
        Id = id;
    }

    public PreviewLineItemException(Guid id, string message, Exception innerException)
        : base(message, innerException)
    {
        Id = id;
    }
}
