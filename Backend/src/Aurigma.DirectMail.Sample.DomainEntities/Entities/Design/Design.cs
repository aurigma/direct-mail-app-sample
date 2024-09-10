using System.Collections.Generic;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.Design;

public class Design
{
    public string Id { get; set; }
    public bool Private { get; set; }
    public long Size { get; set; }
    public string OwnerId { get; set; }
    public string Name { get; set; }
    public IDictionary<string, object> CustomFields { get; set; }
}
