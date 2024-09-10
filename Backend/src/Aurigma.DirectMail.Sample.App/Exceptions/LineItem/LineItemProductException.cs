namespace Aurigma.DirectMail.Sample.App.Exceptions.LineItem;

public class LineItemProductException : Exception
{
    public DomainEntities.Entities.Product.Product Product { get; }

    public LineItemProductException(
        string message,
        DomainEntities.Entities.Product.Product product = null
    )
        : base(message)
    {
        Product = product;
    }
}
