using System;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.Product;

public class Product
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Product title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Product price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Product category id.
    /// </summary>
    public Guid? CategoryId { get; set; }
}
