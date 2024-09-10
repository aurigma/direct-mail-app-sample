using System;
using Aurigma.DirectMail.Sample.App.Adapters.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Adapters.CustomersCanvas.DesignEditor;
using Aurigma.DirectMail.Sample.App.Configurations;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas.DesignEditor;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.App.Services.AppServices;
using Aurigma.DirectMail.Sample.WebHost.Extensions;
using Aurigma.DirectMail.Sample.WebHost.Interfaces;
using Aurigma.DirectMail.Sample.WebHost.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Aurigma.DirectMail.Sample.WebHost.Configs.Components;

/// <summary>
/// Configuration of component <see cref="Aurigma.DirectMail.Sample.App"/>.
/// </summary>
public static class AppServicesConfig
{
    private const string CustomersCanvasConfigurationSection = "CustomersCanvas";

    /// <summary>
    /// Adds configuration for component <see cref="Aurigma.DirectMail.Sample.App"/>
    /// </summary>
    public static void AddAppServicesConfig(
        this IServiceCollection services,
        IConfiguration configuration,
        IStartupLogger logger
    )
    {
        AddAppServices(services);
        AddAdapters(services);
        AddHttpClients(services);
        AddConfigurations(services, configuration, logger);
        AddConfigurationValidators(services);
    }

    private static void AddConfigurations(
        IServiceCollection services,
        IConfiguration configuration,
        IStartupLogger logger
    )
    {
        AddConfigurationSection<CustomersCanvasConfiguration>(
            services,
            configuration,
            logger,
            CustomersCanvasConfigurationSection
        );
    }

    private static void AddAppServices(IServiceCollection services)
    {
        services.AddTransient<ICampaignAppService, CampaignAppService>();
        services.AddTransient<ILineItemAppService, LineItemAppService>();
        services.AddTransient<IProductAppService, ProductAppService>();
        services.AddTransient<ICategoryAppService, CategoryAppService>();
        services.AddTransient<ICompanyAppService, CompanyAppService>();
        services.AddTransient<IIntegratedProductAppService, IntegratedProductAppService>();
        services.AddTransient<IRecipientListAppService, RecipientListAppService>();
        services.AddTransient<IEditorAppService, EditorAppService>();
        services.AddTransient<IPreviewAppService, PreviewAppService>();
        services.AddTransient<IJobAppService, JobAppService>();
    }

    private static void AddAdapters(IServiceCollection services)
    {
        services.AddScoped<ITokenAdapter, TokenAdapter>();
        services.AddTransient<IProductReferenceAdapter, ProductReferenceAdapter>();
        services.AddTransient<IProductAdapter, ProductAdapter>();
        services.AddTransient<IDesignAdapter, DesignAdapter>();
        services.AddTransient<IPrivateDesignAdapter, PrivateDesignAdapter>();
        services.AddTransient<ITenantInfoAdapter, TenantInfoAdapter>();
        services.AddTransient<IStorefrontUserAdapter, StorefrontUserAdapter>();
        services.AddTransient<IDesignEditorAdapter, DesignEditorAdapter>();
        services.AddTransient<IDesignAtomsServiceAdapter, DesignAtomsServiceAdapter>();
        services.AddTransient<IProjectAdapter, ProjectAdapter>();
        services.AddTransient<IPrivateImageAdapter, PrivateImageAdapter>();
        services.AddTransient<IResourceAdapter, ResourceAdapter>();
        services.AddTransient<IPrivateDesignProcessorAdapter, PrivateDesignProcessorAdapter>();
        services.AddTransient<IPrivateImageProcessorAdapter, PrivateImageProcessorAdapter>();
    }

    private static void AddHttpClients(IServiceCollection services)
    {
        services.AddHttpClient<ITokenAdapter, TokenAdapter>();
    }

    private static void AddConfigurationSection<TOptions>(
        IServiceCollection services,
        IConfiguration configuration,
        IStartupLogger logger,
        string section
    )
        where TOptions : class
    {
        try
        {
            if (configuration.IsSectionNotExists(section))
            {
                logger.Warn($"The configuration section '{section}' is not specified");
                return;
            }

            services
                .AddOptions<TOptions>()
                .Bind(configuration.GetSection(section))
                .ValidateOnStart();
        }
        catch (Exception ex)
        {
            logger.Error(ex, ex.Message);
            throw;
        }
    }

    private static void AddConfigurationValidators(IServiceCollection services)
    {
        services.AddSingleton<
            IValidateOptions<CustomersCanvasConfiguration>,
            CustomersCanvasConfigurationValidator
        >();
    }
}
