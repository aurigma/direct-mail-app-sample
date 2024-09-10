namespace Aurigma.DirectMail.Sample.WebHost.Settings;

public class EntityFrameworkSettings
{
    /// <summary>
    /// Indicates if migration should be applied to databases automatically.
    /// </summary>
    public bool ApplyMigrationsOnStartup { get; init; }

    /// <summary>
    /// Tells Entity Framework Core to include the parameter values in its logging messages.
    /// </summary>
    public bool EnableSensitiveDataLogging { get; init; }
}
