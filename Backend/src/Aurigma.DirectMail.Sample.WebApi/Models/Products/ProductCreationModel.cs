using System;

namespace Aurigma.DirectMail.Sample.WebApi.Models.Products;

public class ProductCreationModel
{
    public string Title { get; set; }
    public decimal Price { get; set; }

    public Guid? CategoryId { get; set; }
}
