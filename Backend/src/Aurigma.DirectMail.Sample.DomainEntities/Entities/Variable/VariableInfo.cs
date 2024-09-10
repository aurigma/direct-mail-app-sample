using Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.Variable;

public class VariableInfo
{
    public string Name { get; set; }

    public VariableType Type { get; set; }

    public VariableBarcodeFormat? BarcodeFormat { get; set; }

    public VariableBarcodeSubType? BarcodeSubType { get; set; }
}
