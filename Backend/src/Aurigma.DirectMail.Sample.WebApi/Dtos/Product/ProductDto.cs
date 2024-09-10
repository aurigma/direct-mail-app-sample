using System;

namespace Aurigma.DirectMail.Sample.WebApi.Dtos.Product;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public Guid? CategoryId { get; set; }
}
