using System;
using System.Collections.Generic;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.Category;

public class Category
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public IEnumerable<Guid> ProductIds { get; set; }
}
