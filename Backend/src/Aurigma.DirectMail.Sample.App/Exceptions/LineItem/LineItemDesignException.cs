namespace Aurigma.DirectMail.Sample.App.Exceptions.LineItem;

public class LineItemDesignException : Exception
{
    public string Id { get; set; }

    public LineItemDesignException() { }

    public LineItemDesignException(string id)
    {
        Id = id;
    }

    public LineItemDesignException(string id, string message)
        : base(message)
    {
        Id = id;
    }

    public LineItemDesignException(string id, string message, Exception innerException)
        : base(message, innerException)
    {
        Id = id;
    }
}
