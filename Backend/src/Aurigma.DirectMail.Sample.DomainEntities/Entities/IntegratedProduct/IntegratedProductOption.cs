using System.Collections.Generic;
using Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.IntegratedProduct;

public class IntegratedProductOption
{
    public int Id { get; set; }

    public string Title { get; set; }
    public IntegratedProductOptionType OptionType { get; set; }

    public IEnumerable<IntegratedProductOptionValue> Values { get; set; }
}
