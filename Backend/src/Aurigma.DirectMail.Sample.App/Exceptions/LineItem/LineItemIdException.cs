namespace Aurigma.DirectMail.Sample.App.Exceptions.LineItem;

public class LineItemIdException : Exception
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public Guid Id { get; }

    public LineItemIdException(string message, Guid id)
        : base(message)
    {
        Id = id;
    }
}
