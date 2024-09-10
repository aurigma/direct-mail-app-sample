namespace Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.UnitOfWork;

/// <summary>
/// Unit of work in Repository pattern.
/// </summary>
public interface IUnitOfWork
{
    /// <inheritdoc cref="IGenericUnitOfWork{TContext}.CreateTransaction"/>
    public void CreateTransaction();

    /// <inheritdoc cref="IGenericUnitOfWork{TContext}.Commit"/>
    public void CommitTransaction();

    /// <inheritdoc cref="IGenericUnitOfWork{TContext}.Rollback"/>
    public void RollbackTransaction();
}
