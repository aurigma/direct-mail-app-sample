using System;
using System.Collections.Generic;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Entities;

public class RecipientListDal : IHasCreationTime, IHasModificationTime
{
    /// <summary>
    /// Unique identifier (primary key).
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// List title.
    /// </summary>
    public string Title { get; set; }

    #region Relationships
    public ICollection<RecipientDal> Recipients { get; set; }

    public ICollection<RecipientListCampaignDal> RecipientListCampaigns { get; set; }

    #endregion Relationships

    #region Audit properties
    /// <summary>
    /// Creation time of this entity.
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// The last modified time for this entity.
    /// </summary>
    public DateTime? LastModificationTime { get; set; }
    #endregion Audit properties
}
