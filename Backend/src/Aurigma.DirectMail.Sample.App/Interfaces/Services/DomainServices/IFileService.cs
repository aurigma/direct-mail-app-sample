namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;

using Aurigma.DirectMail.Sample.DomainEntities.Entities.File;
using DomainEntity = DirectMail.Sample.DomainEntities.Entities.File;

/// <summary>
/// Domain service for performing operations with file system.
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Returns a files from folder.
    /// </summary>
    /// <param name="folder">Target folder name.</param>
    /// <returns>The collection of files.</returns>
    Task<IEnumerable<File>> GetFilesAsync(string folder);

    /// <summary>
    /// Returns a file by name.
    /// </summary>
    /// <param name="fileName">File name.</param>
    /// <returns>The file.</returns>
    Task<File> GetFileAsync(string fileName);
}
