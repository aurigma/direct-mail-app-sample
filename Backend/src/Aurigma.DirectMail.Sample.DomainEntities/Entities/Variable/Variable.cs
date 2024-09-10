using Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.Variable;

public class Variable
{
    public string Name { get; set; }

    public string Value { get; set; }

    public VariableType Type { get; set; }

    public VariableBarcodeFormat? BarcodeFormat { get; set; }

    public VariableBarcodeSubType? BarcodeSubType { get; set; }
}
