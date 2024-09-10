using Aurigma.DirectMail.Sample.DAL.EFCore.Context;
using Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Context;
using Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;
using Aurigma.DirectMail.Sample.DAL.EFCore.Repositories;
using Aurigma.DirectMail.Sample.DAL.Postgres.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Aurigma.DirectMail.Sample.WebHost.Configs.Components;

/// <summary>
/// Configuration of component <see cref="Aurigma.DirectMail.Sample.DAL.EFCore"/>.
/// </summary>
public static class EfCoreConfig
{
    /// <summary>
    /// Adds configuration for component <see cref="Aurigma.DirectMail.Sample.DAL.EFCore"/>.
    /// </summary>
    /// <param name="services"></param>
    public static void AddEfCoreConfig(this IServiceCollection services)
    {
        AddContext(services);
        AddRepositories(services);
    }

    private static void AddContext(IServiceCollection services)
    {
        services.AddScoped<IDataContext, ContextFacade<PostgresDataContext>>(
            factory => new ContextFacade<PostgresDataContext>(
                factory.GetService<PostgresDataContext>()
            )
        );
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddTransient<ICampaignRepository, CampaignRepository>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<ICompanyRepository, CompanyRepository>();
        services.AddTransient<ILineItemRepository, LineItemRepository>();
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IRecipientListRepository, RecipientListRepository>();
        services.AddTransient<IJobRepository, JobRepository>();
    }
}
