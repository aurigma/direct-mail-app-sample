namespace Aurigma.DirectMail.Sample.DAL.FileSystem.Entities;

public class FileDal
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

    /// <summary>
    /// File last modified date.
    /// </summary>
    public DateTime LastModifiedTime { get; set; }
}
