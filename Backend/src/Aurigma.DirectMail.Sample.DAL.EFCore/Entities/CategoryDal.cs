using System;
using System.Collections.Generic;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Entities;

public class CategoryDal : IHasCreationTime, IHasModificationTime
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    #region Relationships
    public ICollection<ProductDal> Products { get; set; }

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
