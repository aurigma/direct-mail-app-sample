using Aurigma.DirectMail.Sample.DomainEntities.Entities.Product;

namespace Aurigma.DirectMail.Sample.App.Filters;

/// <summary>
/// Filter for <see cref="Product"/>
/// </summary>
public class ProductFilter
{
    /// <summary>
    /// Product's category id.
    /// </summary>
    public Guid? CategoryId { get; set; }
}
