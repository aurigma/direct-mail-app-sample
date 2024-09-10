using System;
using System.Collections.Generic;
using Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Entities;

public class CampaignDal : IHasCreationTime, IHasModificationTime
{
    /// <summary>
    /// Unique identifier (primary key).
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Campaign type.
    /// </summary>
    public CampaignType Type { get; set; }

    #region Relationships
    public Guid CompanyId { get; set; }
    public CompanyDal Company { get; set; }

    public ICollection<LineItemDal> LineItems { get; set; } = new List<LineItemDal>();
    public ICollection<RecipientListCampaignDal> RecipientListCampaigns { get; set; } =
        new List<RecipientListCampaignDal>();

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
