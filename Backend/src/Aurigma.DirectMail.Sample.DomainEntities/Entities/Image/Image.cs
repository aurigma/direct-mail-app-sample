namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.Image;

public class Image
{
    /// <summary>
    /// File name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Absolute file path.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// File content.
    /// </summary>
    public byte[] Content { get; set; }
}
