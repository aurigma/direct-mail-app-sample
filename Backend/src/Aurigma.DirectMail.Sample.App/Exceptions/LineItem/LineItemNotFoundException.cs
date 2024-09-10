namespace Aurigma.DirectMail.Sample.App.Exceptions.LineItem;

public class LineItemNotFoundException : Exception
{
    public Guid Id { get; set; }

    public LineItemNotFoundException() { }

    public LineItemNotFoundException(Guid id)
    {
        Id = id;
    }

    public LineItemNotFoundException(Guid id, string message)
        : base(message)
    {
        Id = id;
    }

    public LineItemNotFoundException(Guid id, string message, Exception innerException)
        : base(message, innerException)
    {
        Id = id;
    }
}
