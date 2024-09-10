namespace Aurigma.DirectMail.Sample.App.Exceptions.LineItem;

public class LineItemProductVariantException : Exception
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public string Id { get; }

    public LineItemProductVariantException(string message, string id)
        : base(message)
    {
        Id = id;
    }
}
