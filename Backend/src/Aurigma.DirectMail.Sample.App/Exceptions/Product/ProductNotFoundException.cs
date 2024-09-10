namespace Aurigma.DirectMail.Sample.App.Exceptions.Product;

public class ProductNotFoundException : Exception
{
    public Guid Id { get; set; }

    public ProductNotFoundException() { }

    public ProductNotFoundException(Guid id)
    {
        Id = id;
    }

    public ProductNotFoundException(Guid id, string message)
        : base(message)
    {
        Id = id;
    }

    public ProductNotFoundException(Guid id, string message, Exception innerException)
        : base(message, innerException)
    {
        Id = id;
    }
}
