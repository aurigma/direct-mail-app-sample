namespace Aurigma.DirectMail.Sample.App.Exceptions.Editor;

public class EditorIdException : Exception
{
    public string Id { get; set; }

    public EditorIdException(string message, string id)
        : base(message)
    {
        Id = id;
    }
}
