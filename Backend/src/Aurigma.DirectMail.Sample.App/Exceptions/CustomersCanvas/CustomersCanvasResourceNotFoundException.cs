namespace Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;

public class CustomersCanvasResourceNotFoundException : Exception
{
    public string Id { get; set; }

    public CustomersCanvasResourceNotFoundException() { }

    public CustomersCanvasResourceNotFoundException(string id)
    {
        Id = id;
    }

    public CustomersCanvasResourceNotFoundException(string id, string message)
        : base(message)
    {
        Id = id;
    }

    public CustomersCanvasResourceNotFoundException(
        string id,
        string message,
        Exception innerException
    )
        : base(message, innerException)
    {
        Id = id;
    }
}
