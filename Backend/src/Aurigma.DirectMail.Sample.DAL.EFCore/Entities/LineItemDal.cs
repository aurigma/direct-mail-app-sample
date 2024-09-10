using System;
using System.Collections.Generic;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Entities;

public class LineItemDal : IHasCreationTime, IHasModificationTime
{
    public Guid Id { get; set; }

    public int Quantity { get; set; }

    public string TemplateId { get; set; }

    public string DesignId { get; set; }

    public int? ProductVariantId { get; set; }

    #region Relationships
    public Guid? ProductId { get; set; }
    public ProductDal Product { get; set; }

    public Guid? CampaignId { get; set; }
    public CampaignDal Campaign { get; set; }

    public ICollection<JobDal> Jobs { get; set; }
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
