using System.IO.Abstractions;
using Aurigma.DirectMail.Sample.DAL.FileSystem.Interfaces.Repositories;
using Aurigma.DirectMail.Sample.DAL.FileSystem.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Aurigma.DirectMail.Sample.WebHost.Configs.Components;

/// <summary>
/// Configuration of component <see cref="Aurigma.DirectMail.Sample.DAL.FileSystem"/>
/// </summary>
public static class FileSystemConfig
{
    /// <summary>
    /// Adds configuration for component <see cref="Aurigma.DirectMail.Sample.DAL.FileSystem"/>.
    /// </summary>
    /// <param name="services"></param>
    public static void AddFileSystemConfig(this IServiceCollection services)
    {
        AddFileSystem(services);
        AddRepositories(services);
    }

    private static void AddFileSystem(IServiceCollection services)
    {
        services.AddSingleton<IFileSystem, FileSystem>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddTransient<IFileRepository, FileRepository>();
    }
}
