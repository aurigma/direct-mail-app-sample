namespace Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas.DesignEditor;

public class DesignEditorAdapterException : Exception
{
    public int StatusCode { get; set; }

    public DesignEditorAdapterException(int statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }
}
