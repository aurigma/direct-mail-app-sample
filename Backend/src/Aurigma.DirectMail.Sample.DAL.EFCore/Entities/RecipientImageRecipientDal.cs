using System;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Entities;

public class RecipientImageRecipientDal
{
    public Guid RecipientId { get; set; }

    public Guid RecipientImageId { get; set; }

    public RecipientDal Recipient { get; set; }

    public RecipientImageDal RecipientImage { get; set; }
}
