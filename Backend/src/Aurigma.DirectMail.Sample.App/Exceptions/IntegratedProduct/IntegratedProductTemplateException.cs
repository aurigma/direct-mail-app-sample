namespace Aurigma.DirectMail.Sample.App.Exceptions.IntegratedProduct;

public class IntegratedProductTemplateException : Exception
{
    public IntegratedProductTemplateException() { }

    public IntegratedProductTemplateException(string message)
        : base(message) { }

    public IntegratedProductTemplateException(string message, Exception innerException)
        : base(message, innerException) { }
}
