using Aurigma.DirectMail.Sample.App.Exceptions.Company;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Company;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;

/// <summary>
/// Application service for performing operations with companies.
/// </summary>
public interface ICompanyService
{
    /// <summary>
    /// Creates a new company.
    /// </summary>
    /// <param name="model">Model to create a company.</param>
    /// <returns>The created company.</returns>
    Task<Company> CreateCompanyAsync(Company company);

    /// <summary>
    /// Returns a company by id.
    /// </summary>
    /// <param name="id">ID of the compay to search for.</param>
    /// <returns>The found company.</returns>
    /// <exception cref="CompanyNotFoundException"> The company was not found.</exception>
    Task<Company> GetCompanyAsync(Guid id);

    /// <summary>
    /// Return all companies by filter.
    /// </summary>
    /// <param name="filter">Request filter model.</param>
    Task<List<Company>> GetCompaniesAsync();
}
