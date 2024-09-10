namespace Aurigma.DirectMail.Sample.App.Interfaces.Storages;

using Aurigma.DirectMail.Sample.DomainEntities.Entities.File;
using DomainEntity = DomainEntities.Entities.File;

public interface IFileRepository
{
    Task<IEnumerable<File>> GetFilesAsync(string targetFolder);
    Task<File> GetFileAsync(string filePath);
}
