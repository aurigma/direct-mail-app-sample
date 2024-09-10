using Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.Editor;

public class EditorVariableInfo
{
    public string Name { get; set; }

    public EditorVariableInfoType Type { get; set; }

    public EditorVariableInfoBarcodeSubType? BarcodeSubType { get; set; }

    public EditorVariableInfoBarcodeFormat? BarcodeFormat { get; set; }
}
