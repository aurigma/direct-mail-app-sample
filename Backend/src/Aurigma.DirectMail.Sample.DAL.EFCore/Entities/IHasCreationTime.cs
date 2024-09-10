using System;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Entities;

public interface IHasCreationTime
{
    /// <summary>
    /// Creation time of this entity.
    /// </summary>
    public DateTime CreationTime { get; set; }
}

public interface IHasModificationTime
{
    /// <summary>
    /// The last modified time for this entity.
    /// </summary>
    public DateTime? LastModificationTime { get; set; }
}
