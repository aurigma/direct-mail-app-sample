using System.IO;
using System.IO.Abstractions;
using Aurigma.DirectMail.Sample.DAL.FileSystem.Entities;
using Aurigma.DirectMail.Sample.DAL.FileSystem.Interfaces.Repositories;

namespace Aurigma.DirectMail.Sample.DAL.FileSystem.Repositories;

public class FileRepository(IFileSystem fileSystem) : IFileRepository
{
    private readonly IFileSystem _fileSystem = fileSystem;

    public async Task<IEnumerable<FileDal>> GetFilesAsync(string targetFolder)
    {
        var assemblyLocation = AppDomain.CurrentDomain.BaseDirectory;

        if (string.IsNullOrWhiteSpace(assemblyLocation))
        {
            throw new FileNotFoundException("Error getting current assembly path.");
        }

        var files = new List<FileDal>();
        foreach (
            var filePath in _fileSystem.Directory.EnumerateFiles(
                Path.Combine(assemblyLocation, targetFolder),
                "*",
                SearchOption.AllDirectories
            )
        )
        {
            var formattedFilePath = filePath.Replace('\\', '/');
            var fileLastModifierTime = GetFileLastWriteTime(formattedFilePath);
            var fileName = Path.GetFileName(formattedFilePath);
            var fileContent = await GetFileContent(formattedFilePath);
            files.Add(
                new FileDal
                {
                    Name = fileName,
                    Path = formattedFilePath,
                    Content = fileContent,
                    LastModifiedTime = fileLastModifierTime,
                }
            );
        }

        return files;
    }

    public async Task<FileDal> GetFileAsync(string filePath)
    {
        var assemblyLocation = AppDomain.CurrentDomain.BaseDirectory;

        if (string.IsNullOrWhiteSpace(assemblyLocation))
        {
            throw new FileNotFoundException("Error getting current assembly path.");
        }

        var formattedFilePath = filePath.Replace('\\', '/');
        var file = _fileSystem.File;
        if (!file.Exists(formattedFilePath))
        {
            throw new FileNotFoundException("File does not exist", formattedFilePath);
        }

        var fileName = Path.GetFileName(formattedFilePath);
        var fileLastModifierTime = GetFileLastWriteTime(formattedFilePath);
        var binaryContent = await file.ReadAllBytesAsync(formattedFilePath);

        var fileDal = new FileDal
        {
            Name = fileName,
            Path = formattedFilePath,
            Content = binaryContent,
            LastModifiedTime = fileLastModifierTime,
        };

        return fileDal;
    }

    private DateTime GetFileLastWriteTime(string path)
    {
        var file = _fileSystem.File;
        if (!file.Exists(path))
        {
            throw new FileNotFoundException("File does not exist", path);
        }

        var lastModifierTime = file.GetLastWriteTimeUtc(path);

        return lastModifierTime;
    }

    private async Task<byte[]> GetFileContent(string path)
    {
        var file = _fileSystem.File;
        if (!file.Exists(path))
        {
            throw new FileNotFoundException("File does not exist", path);
        }

        var content = await file.ReadAllBytesAsync(path);
        return content;
    }
}
