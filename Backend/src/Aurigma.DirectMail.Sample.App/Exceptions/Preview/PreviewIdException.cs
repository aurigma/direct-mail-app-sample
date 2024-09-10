namespace Aurigma.DirectMail.Sample.App.Exceptions.Preview;

public class PreviewIdException : Exception
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public Guid Id { get; }

    public PreviewIdException(string message, Guid id)
        : base(message)
    {
        Id = id;
    }
}
