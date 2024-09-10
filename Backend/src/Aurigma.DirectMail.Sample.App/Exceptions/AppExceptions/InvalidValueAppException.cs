namespace Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;

public class InvalidValueAppException : BaseAppException
{
    public InvalidValueAppException() { }

    public InvalidValueAppException(string message)
        : base(message) { }

    public InvalidValueAppException(string message, Exception innerException)
        : base(message, innerException) { }
}
