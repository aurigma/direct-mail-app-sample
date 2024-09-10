namespace Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;

public class CustomersCanvasDesignNotFoundException : Exception
{
    public string Id { get; set; }

    public CustomersCanvasDesignNotFoundException() { }

    public CustomersCanvasDesignNotFoundException(string id)
    {
        Id = id;
    }

    public CustomersCanvasDesignNotFoundException(string id, string message)
        : base(message)
    {
        Id = id;
    }

    public CustomersCanvasDesignNotFoundException(
        string id,
        string message,
        Exception innerException
    )
        : base(message, innerException)
    {
        Id = id;
    }
}
