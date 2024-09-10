using System;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Entities;

public class RecipientListCampaignDal
{
    public Guid RecipientListId { get; set; }
    public Guid CampaignId { get; set; }

    public RecipientListDal RecipientList { get; set; }

    public CampaignDal Campaign { get; set; }
}
