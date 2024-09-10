using System.Collections.Generic;
using System.Linq;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.Editor;

public class DesignValidationResult
{
    public IEnumerable<string> MissingListVariableNames { get; init; }

    public IEnumerable<string> MissingDesignVariableNames { get; init; }

    public bool IsValid
    {
        get =>
            MissingListVariableNames.Count() > 0 || MissingDesignVariableNames.Count() > 0
                ? false
                : true;
    }
}
