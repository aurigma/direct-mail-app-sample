using System;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.IntegratedProduct;

public class IntegratedProduct
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
    /// Customer's canvas preview url.
    /// </summary>
    public string PreviewUrl { get; set; }

    /// <summary>
    /// Product ID in Customers Canvas.
    /// </summary>
    public int CustomersCanvasProductId { get; set; }

    /// <summary>
    /// Product category id.
    /// </summary>
    public Guid? CategoryId { get; set; }
}
