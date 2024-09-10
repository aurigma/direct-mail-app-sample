using System;
using Aurigma.DirectMail.Sample.WebHost.Extensions;
using Aurigma.DirectMail.Sample.WebHost.Interfaces;
using Aurigma.DirectMail.Sample.WebHost.Settings;
using Aurigma.StorefrontApi;
using Aurigma.StorefrontApi.Products;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurigma.DirectMail.Sample.WebHost.Configs.Common;

/// <summary>
/// Customers Canvas API configuration.
/// </summary>
public static class CustomersCanvasConfig
{
    private const string CustomersCanvasConfigSection = "CustomersCanvas";

    /// <summary>
    /// Adds a Customers Canvas configuration.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
    /// <param name="logger">The <see cref="IStartupLogger"/>.</param>
    public static void AddCustomersCanvasApiClientConfig(
        this IServiceCollection services,
        IConfiguration configuration,
        IStartupLogger logger
    )
    {
        if (configuration.IsSectionNotExists(CustomersCanvasConfigSection))
        {
            logger.Warn(
                $"The configuration section {CustomersCanvasConfigSection} is not specified"
            );
            return;
        }
        var customerCanvasSettings = configuration.CastSection<CustomersCanvasSettings>(
            CustomersCanvasConfigSection
        );

        if (string.IsNullOrWhiteSpace(customerCanvasSettings.ApiGatewayUrl))
        {
            throw new Exception(
                $"The configuration parameter '{CustomersCanvasConfigSection}:"
                    + $"{nameof(CustomersCanvasSettings.ApiGatewayUrl)}' is not specified"
            );
        }

        SetStorefrontApiClientConfiguration(services, customerCanvasSettings);
        SetAssetStorageApiClientConfiguration(services, customerCanvasSettings);
        SetDesignAtomsApiClientConfiguration(services, customerCanvasSettings);
        SetAssetProcessorApiClientConfiguration(services, customerCanvasSettings);
        RegisterApiClients(services);
    }

    private static void SetStorefrontApiClientConfiguration(
        IServiceCollection services,
        CustomersCanvasSettings settings
    )
    {
        services.AddSingleton<IApiClientConfiguration>(
            new ApiClientConfiguration { ApiUrl = settings.ApiGatewayUrl }
        );
    }

    private static void SetAssetStorageApiClientConfiguration(
        IServiceCollection services,
        CustomersCanvasSettings settings
    )
    {
        services.AddSingleton<AssetStorage.IApiClientConfiguration>(
            new AssetStorage.ApiClientConfiguration { ApiUrl = settings.ApiGatewayUrl }
        );
    }

    private static void SetDesignAtomsApiClientConfiguration(
        IServiceCollection services,
        CustomersCanvasSettings settings
    )
    {
        services.AddSingleton<DesignAtomsApi.IApiClientConfiguration>(
            new DesignAtomsApi.ApiClientConfiguration { ApiUrl = settings.ApiGatewayUrl }
        );
    }

    private static void SetAssetProcessorApiClientConfiguration(
        IServiceCollection services,
        CustomersCanvasSettings settings
    )
    {
        services.AddSingleton<AssetProcessor.IApiClientConfiguration>(
            new AssetProcessor.ApiClientConfiguration { ApiUrl = settings.ApiGatewayUrl }
        );
    }

    private static void RegisterApiClients(IServiceCollection services)
    {
        // Storefront API clients.
        services.AddTransient<IProductsApiClient, ProductsApiClient>();
        services.AddTransient<
            StorefrontApi.Products.IProductReferencesApiClient,
            StorefrontApi.Products.ProductReferencesApiClient
        >();
        services.AddTransient<ITenantInfoApiClient, TenantInfoApiClient>();
        services.AddTransient<IStorefrontUsersApiClient, StorefrontUsersApiClient>();
        services.AddTransient<IProjectsApiClient, ProjectsApiClient>();

        // Asset Storage API clients.
        services.AddTransient<AssetStorage.IDesignsApiClient, AssetStorage.DesignsApiClient>();
        services.AddTransient<
            AssetStorage.IPrivateDesignsApiClient,
            AssetStorage.PrivateDesignsApiClient
        >();
        services.AddTransient<
            AssetStorage.IPrivateImagesApiClient,
            AssetStorage.PrivateImagesApiClient
        >();
        services.AddTransient<AssetStorage.IResourcesApiClient, AssetStorage.ResourcesApiClient>();

        // Design Atoms API clients.
        services.AddTransient<
            DesignAtomsApi.IDesignAtomsServiceApiClient,
            DesignAtomsApi.DesignAtomsServiceApiClient
        >();

        // Asset Processor API clients.
        services.AddTransient<
            AssetProcessor.IPrivateDesignProcessorApiClient,
            AssetProcessor.PrivateDesignProcessorApiClient
        >();
        services.AddTransient<
            AssetProcessor.IPrivateImageProcessorApiClient,
            AssetProcessor.PrivateImageProcessorApiClient
        >();
    }
}
