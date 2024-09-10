namespace Aurigma.DirectMail.Sample.App.Exceptions.IntegratedProduct;

public class IntegratedProductOptionsException : Exception
{
    public IntegratedProductOptionsException() { }

    public IntegratedProductOptionsException(string message)
        : base(message) { }

    public IntegratedProductOptionsException(string message, Exception innerException)
        : base(message, innerException) { }
}
