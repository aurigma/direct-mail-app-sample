namespace Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;

public class NotFoundAppException : BaseAppException
{
    public NotFoundAppException() { }

    public NotFoundAppException(string message)
        : base(message) { }

    public NotFoundAppException(string message, Exception innerException)
        : base(message, innerException) { }
}
