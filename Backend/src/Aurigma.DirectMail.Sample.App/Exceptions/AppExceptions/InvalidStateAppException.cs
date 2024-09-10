namespace Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;

public class InvalidStateAppException : Exception
{
    public string PropertyName { get; set; }
    public string PropertyValue { get; set; }

    public InvalidStateAppException() { }

    public InvalidStateAppException(string message)
        : base(message) { }

    public InvalidStateAppException(string message, Exception innerException)
        : base(message, innerException) { }

    public InvalidStateAppException(string propertyName, string message, Exception innerException)
        : base(message, innerException)
    {
        PropertyName = propertyName;
    }

    public InvalidStateAppException(
        string propertyName,
        string propertyValue,
        string message,
        Exception innerException
    )
        : base(message, innerException)
    {
        PropertyName = propertyName;
        PropertyValue = propertyValue;
    }
}
