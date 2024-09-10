using Aurigma.DirectMail.Sample.App.Configurations;
using Microsoft.Extensions.Options;

namespace Aurigma.DirectMail.Sample.WebHost.Validators;

public class CustomersCanvasConfigurationValidator : IValidateOptions<CustomersCanvasConfiguration>
{
    public ValidateOptionsResult Validate(string name, CustomersCanvasConfiguration options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail(
                $"{typeof(CustomersCanvasConfiguration)} object is null"
            );
        }

        if (string.IsNullOrWhiteSpace(options.ClientId))
        {
            return ValidateOptionsResult.Fail(
                $"The configuration parameter '{typeof(CustomersCanvasConfiguration)}:"
                    + $"{nameof(CustomersCanvasConfiguration.ClientId)}' is not specified"
            );
        }

        if (string.IsNullOrWhiteSpace(options.ClientSecret))
        {
            return ValidateOptionsResult.Fail(
                $"The configuration parameter '{typeof(CustomersCanvasConfiguration)}:"
                    + $"{nameof(CustomersCanvasConfiguration.ClientSecret)}' is not specified"
            );
        }

        if (options.TenantId == default)
        {
            return ValidateOptionsResult.Fail(
                $"The configuration parameter '{typeof(CustomersCanvasConfiguration)}:"
                    + $"{nameof(CustomersCanvasConfiguration.TenantId)}' is not specified"
            );
        }

        if (options.StorefrontId == default)
        {
            return ValidateOptionsResult.Fail(
                $"The configuration parameter '{typeof(CustomersCanvasConfiguration)}:"
                    + $"{nameof(CustomersCanvasConfiguration.StorefrontId)}' is not specified"
            );
        }

        return ValidateOptionsResult.Success;
    }
}
