using System;
using System.Collections.Generic;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.RecipientList;

public class RecipientList
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public IEnumerable<Recipient> Recipients { get; set; }
}
