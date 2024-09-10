using System;
using System.Collections.Generic;

namespace Aurigma.DirectMail.Sample.WebApi.Dtos.RecipientList;

public class RecipientListDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public IEnumerable<RecipientDto> Recipients { get; set; }
}
