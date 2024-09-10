using System;
using System.Collections.Generic;

namespace Aurigma.DirectMail.Sample.WebApi.Models.RecipientList;

public class RecipientListSubmitModel
{
    public Guid CampaignId { get; set; }
    public IEnumerable<Guid> RecipientListIds { get; set; }
}
