using System.Collections.Generic;

namespace Aurigma.DirectMail.Sample.WebApi.Dtos.Editor;

public class DesignValidationResultDto
{
    public IEnumerable<string> MissingListVariableNames { get; init; }

    public IEnumerable<string> MissingDesignVariableNames { get; init; }

    public bool IsValid { get; init; }
}
