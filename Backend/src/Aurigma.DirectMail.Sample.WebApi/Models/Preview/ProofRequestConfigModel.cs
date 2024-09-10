namespace Aurigma.DirectMail.Sample.WebApi.Models.Preview;

/// <summary>
/// Rendering preview config.
/// </summary>
public class ProofRequestConfigModel
{
    /// <summary>
    /// Preview width.
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Preview height.
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Design's surface index.
    /// </summary>
    public int SurfaceIndex { get; set; }
}
