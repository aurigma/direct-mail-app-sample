using System;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.File;

public class File
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
