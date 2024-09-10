namespace Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Context;

/// <summary>
/// Provides access to repositories for working with database entities and saving changes to the database.
/// </summary>
public interface IDataContext
{
    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <remarks>
    /// Transaction will be created automatically if the 'BeginTransaction' method has not been called.
    /// </remarks>
    void SaveChanges();
}
