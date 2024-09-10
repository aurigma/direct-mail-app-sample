namespace Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;

public class CustomersCanvasDesignNotConnectedException : Exception
{
    public string DesignId { get; set; }
    public string ProductReference { get; set; }

    public CustomersCanvasDesignNotConnectedException() { }

    public CustomersCanvasDesignNotConnectedException(string designId, string productReference)
    {
        DesignId = designId;
        ProductReference = productReference;
    }

    public CustomersCanvasDesignNotConnectedException(
        string designId,
        string productReference,
        string message
    )
        : base(message)
    {
        DesignId = designId;
        ProductReference = productReference;
    }

    public CustomersCanvasDesignNotConnectedException(
        string designId,
        string productReference,
        string message,
        Exception innerException
    )
        : base(message, innerException)
    {
        DesignId = designId;
        ProductReference = productReference;
    }
}
