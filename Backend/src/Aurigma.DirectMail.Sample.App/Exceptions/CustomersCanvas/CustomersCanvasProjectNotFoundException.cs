namespace Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;

public class CustomersCanvasProjectNotFoundException : Exception
{
    public int Id { get; set; }

    public CustomersCanvasProjectNotFoundException() { }

    public CustomersCanvasProjectNotFoundException(int id)
    {
        Id = id;
    }

    public CustomersCanvasProjectNotFoundException(int id, string message)
        : base(message)
    {
        Id = id;
    }

    public CustomersCanvasProjectNotFoundException(int id, string message, Exception innerException)
        : base(message, innerException)
    {
        Id = id;
    }
}
