using System;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Repositories;

public class BaseRepository
{
    protected BaseRepository(DbContext context)
    {
        DataContext = context;
    }

    protected readonly DbContext DataContext;

    protected void PrepareForCreation(IHasCreationTime entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        entity.CreationTime = DateTime.UtcNow;
    }

    protected void PrepareForUpdate(IHasModificationTime entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        entity.LastModificationTime = DateTime.UtcNow;
    }
}
