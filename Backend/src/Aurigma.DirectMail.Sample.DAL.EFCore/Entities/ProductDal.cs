using System;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Entities;

public class ProductDal : IHasCreationTime, IHasModificationTime
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }

    #region Relationships
    public Guid? CategoryId { get; set; }
    public CategoryDal Category { get; set; }

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
