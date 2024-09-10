using Aurigma.DirectMail.Sample.WebApi.Enums;

namespace Aurigma.DirectMail.Sample.WebApi.Dtos.Editor;

public class EditorVariableInfoDto
{
    /// <summary>
    /// Variable name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Variable type in Design Editor workflow.
    /// </summary>
    public EditorVariableInfoType Type { get; set; }

    public EditorVariableInfoBarcodeFormat? BarcodeFormat { get; set; }

    public EditorVariableInfoBarcodeSubType? BarcodeSubType { get; set; }
}
