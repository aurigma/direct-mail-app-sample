namespace Aurigma.DirectMail.Sample.App.Models.Product;

public class ProductCreationModel
{
    public string Title { get; set; }
    public decimal Price { get; set; }

    public Guid? CategoryId { get; set; }
}
