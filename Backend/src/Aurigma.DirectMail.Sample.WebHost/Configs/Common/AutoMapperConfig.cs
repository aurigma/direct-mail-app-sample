using Microsoft.Extensions.DependencyInjection;

namespace Aurigma.DirectMail.Sample.WebHost.Configs.Common;

public static class AutoMapperConfig
{
    public static void AddAutoMapperConfig(this IServiceCollection services)
    {
        const string solutionName = $"{nameof(Aurigma)}.{nameof(DirectMail)}.{nameof(Sample)}";

        services.AddAutoMapper(x =>
            x.AddMaps(
                $"{solutionName}.{nameof(App)}",
                $"{solutionName}.{nameof(WebApi)}",
                $"{solutionName}.{nameof(DAL)}.{nameof(DAL.EFCore)}",
                $"{solutionName}.{nameof(DAL)}.{nameof(DAL.Postgres)}",
                $"{solutionName}.{nameof(DAL)}.{nameof(DAL.FileSystem)}"
            )
        );
    }
}
