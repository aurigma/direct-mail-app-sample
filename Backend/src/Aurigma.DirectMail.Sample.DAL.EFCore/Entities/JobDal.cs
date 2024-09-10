using System;
using Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Entities;

public class JobDal : IHasCreationTime, IHasModificationTime
{
    public Guid Id { get; set; }

    public int CustomersCanvasProjectId { get; set; }

    public JobStatus Status { get; set; }

    #region Relationships
    public Guid? LineItemId { get; set; }
    public LineItemDal LineItem { get; set; }
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
