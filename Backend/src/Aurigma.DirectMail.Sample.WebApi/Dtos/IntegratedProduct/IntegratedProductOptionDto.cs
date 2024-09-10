using System.Collections.Generic;
using Aurigma.DirectMail.Sample.WebApi.Enums;

namespace Aurigma.DirectMail.Sample.WebApi.Dtos.IntegratedProduct;

public class IntegratedProductOptionDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public IntegratedProductOptionType OptionType { get; set; }

    public IEnumerable<IntegratedProductOptionValueDto> Values { get; set; }
}
