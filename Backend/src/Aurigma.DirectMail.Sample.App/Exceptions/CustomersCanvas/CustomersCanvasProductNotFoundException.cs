namespace Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;

public class CustomersCanvasProductNotFoundException : Exception
{
    public string Id { get; set; }

    public CustomersCanvasProductNotFoundException() { }

    public CustomersCanvasProductNotFoundException(string id)
    {
        Id = id;
    }

    public CustomersCanvasProductNotFoundException(string id, string message)
        : base(message)
    {
        Id = id;
    }

    public CustomersCanvasProductNotFoundException(
        string id,
        string message,
        Exception innerException
    )
        : base(message, innerException)
    {
        Id = id;
    }
}
