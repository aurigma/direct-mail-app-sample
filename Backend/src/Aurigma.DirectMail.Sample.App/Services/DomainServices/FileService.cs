using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Storages;
using DomainEntity = Aurigma.DirectMail.Sample.DomainEntities.Entities.File;

namespace Aurigma.DirectMail.Sample.App.Services.DomainServices;

public class FileService(IFileRepository fileRepository) : IFileService
{
    private readonly IFileRepository _fileRepository = fileRepository;

    public async Task<IEnumerable<DomainEntities.Entities.File.File>> GetFilesAsync(string folder)
    {
        return await _fileRepository.GetFilesAsync(folder);
    }

    public async Task<DomainEntities.Entities.File.File> GetFileAsync(string fileName)
    {
        return await _fileRepository.GetFileAsync(fileName);
    }
}
