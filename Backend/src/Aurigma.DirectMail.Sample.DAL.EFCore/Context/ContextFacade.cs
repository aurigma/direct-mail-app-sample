using System;
using Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Context;
using Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Context;

/// <summary>
/// Entity Framework implementation of <see cref="IDataContext"/> and <see cref="IUnitOfWork"/>.
/// </summary>
public class ContextFacade<TContext>(TContext context) : IDataContext, IUnitOfWork, IDisposable
    where TContext : DbContext
{
    private readonly TContext _context = context;

    private bool _disposed;
    private IDbContextTransaction _transaction;

    public void CommitTransaction()
    {
        _transaction.Commit();
    }

    public void CreateTransaction()
    {
        _transaction = _context.Database.BeginTransaction();
    }

    public void RollbackTransaction()
    {
        _transaction.Rollback();
        _transaction.Dispose();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _context.Dispose();
        }

        _disposed = true;
    }
}
