namespace Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;

public class CustomersCanvasProductVariantNotFoundException : Exception
{
    public string Id { get; set; }

    public CustomersCanvasProductVariantNotFoundException() { }

    public CustomersCanvasProductVariantNotFoundException(string id)
    {
        Id = id;
    }

    public CustomersCanvasProductVariantNotFoundException(string id, string message)
        : base(message)
    {
        Id = id;
    }

    public CustomersCanvasProductVariantNotFoundException(
        string id,
        string message,
        Exception innerException
    )
        : base(message, innerException)
    {
        Id = id;
    }
}
