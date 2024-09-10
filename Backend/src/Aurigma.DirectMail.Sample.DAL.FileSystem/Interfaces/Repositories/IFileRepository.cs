using Aurigma.DirectMail.Sample.DAL.FileSystem.Entities;
using DomainEntity = Aurigma.DirectMail.Sample.DomainEntities.Entities.File;

namespace Aurigma.DirectMail.Sample.DAL.FileSystem.Interfaces.Repositories;

public interface IFileRepository
{
    Task<IEnumerable<FileDal>> GetFilesAsync(string targetFolder);
    Task<FileDal> GetFileAsync(string filePath);
}
