namespace Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;

public class CustomersCanvasPrivateDesignNotFoundException : Exception
{
    public string Id { get; set; }

    public CustomersCanvasPrivateDesignNotFoundException() { }

    public CustomersCanvasPrivateDesignNotFoundException(string id)
    {
        Id = id;
    }

    public CustomersCanvasPrivateDesignNotFoundException(string id, string message)
        : base(message)
    {
        Id = id;
    }

    public CustomersCanvasPrivateDesignNotFoundException(
        string id,
        string message,
        Exception innerException
    )
        : base(message, innerException)
    {
        Id = id;
    }
}
