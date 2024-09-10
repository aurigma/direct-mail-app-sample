namespace Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;

public abstract class BaseAppException : Exception
{
    public BaseAppException()
        : base() { }

    public BaseAppException(string message)
        : base(message) { }

    public BaseAppException(string message, Exception innerException)
        : base(message, innerException) { }
}
