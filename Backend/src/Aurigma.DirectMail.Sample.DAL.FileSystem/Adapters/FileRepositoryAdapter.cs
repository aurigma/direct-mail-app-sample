using Aurigma.DirectMail.Sample.DAL.FileSystem.Entities;
using Aurigma.DirectMail.Sample.DAL.FileSystem.Interfaces.Repositories;
using AutoMapper;
using DomainEntity = Aurigma.DirectMail.Sample.DomainEntities.Entities.File;
using DomainStorage = Aurigma.DirectMail.Sample.App.Interfaces.Storages;

namespace Aurigma.DirectMail.Sample.DAL.FileSystem.Adapters;

public class FileRepositoryAdapter(IFileRepository fileRepository, IMapper mapper)
    : DomainStorage.IFileRepository
{
    private readonly IFileRepository _fileRepository = fileRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<DomainEntity.File>> GetFilesAsync(string targetFolder)
    {
        return _mapper.Map<IEnumerable<DomainEntity.File>>(
            await _fileRepository.GetFilesAsync(targetFolder)
        );
    }

    public async Task<DomainEntity.File> GetFileAsync(string filePath)
    {
        return _mapper.Map<DomainEntity.File>(await _fileRepository.GetFileAsync(filePath));
    }
}

internal class FileRepositoryAdapterMapperProfile : Profile
{
    public FileRepositoryAdapterMapperProfile()
    {
        CreateMap<FileDal, DomainEntity.File>();
        CreateMap<DomainEntity.File, FileDal>();
    }
}
