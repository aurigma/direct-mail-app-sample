using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Storages;
using Aurigma.DirectMail.Sample.App.Services.DomainServices;
using Aurigma.DirectMail.Sample.DAL.EFCore.Adapters.Repositories;
using Aurigma.DirectMail.Sample.DAL.FileSystem.Adapters;
using Microsoft.Extensions.DependencyInjection;

namespace Aurigma.DirectMail.Sample.WebHost.Configs.Components;

/// <summary>
/// Configuration of Domain component.
/// </summary>
public static class DomainServicesConfig
{
    /// <summary>
    /// Adds configuration for Domain component.
    /// </summary>
    /// <param name="services"></param>
    public static void AddDomainServicesConfig(this IServiceCollection services)
    {
        AddDomainServices(services);
        AddRepositories(services);
    }

    private static void AddDomainServices(IServiceCollection services)
    {
        services.AddTransient<ICampaignService, CampaignService>();
        services.AddTransient<ILineItemService, LineItemService>();
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<ICompanyService, CompanyService>();
        services.AddTransient<IIntegratedProductService, IntegratedProductService>();
        services.AddTransient<IRecipientListService, RecipientListService>();
        services.AddTransient<IEditorService, EditorService>();
        services.AddTransient<IVdpService, VdpService>();
        services.AddTransient<IPreviewService, PreviewService>();
        services.AddTransient<IJobService, JobService>();
        services.AddTransient<IProjectService, ProjectService>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IImageService, ImageService>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddTransient<ICampaignRepository, CampaignRepositoryAdapter>();
        services.AddTransient<ICategoryRepository, CategoryRepositoryAdapter>();
        services.AddTransient<ICompanyRepository, CompanyRepositoryAdapter>();
        services.AddTransient<ILineItemRepository, LineItemRepositoryAdapter>();
        services.AddTransient<IProductRepository, ProductRepositoryAdapter>();
        services.AddTransient<IRecipientListRepository, RecipientListAdapter>();
        services.AddTransient<IJobRepository, JobRepositoryAdapter>();
        services.AddTransient<IFileRepository, FileRepositoryAdapter>();
    }
}
