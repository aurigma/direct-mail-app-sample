using System.Collections.Generic;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.Variable;

public class VariableValidationResult
{
    public List<string> MissingRecipientVariableNames { get; init; }
    public List<string> MissingSourceVariableNames { get; init; }
}
