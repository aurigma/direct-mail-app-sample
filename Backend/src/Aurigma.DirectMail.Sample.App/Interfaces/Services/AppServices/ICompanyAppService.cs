using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Models.Company;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Company;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;

public interface ICompanyAppService
{
    /// <summary>
    /// Creates a new company.
    /// </summary>
    /// <param name="model">Model to create a company.</param>
    /// <returns>The created company.</returns>
    Task<Company> CreateCompanyAsync(CompanyCreationModel model);

    /// <summary>
    /// Returns a company by id.
    /// </summary>
    /// <param name="id">ID of the compay to search for.</param>
    /// <returns>The found company.</returns>
    /// <exception cref="NotFoundAppException"> The company was not found.</exception>
    Task<Company> GetCompanyAsync(Guid id);

    /// <summary>
    /// Return all companies by filter.
    /// </summary>
    /// <param name="filter">Request filter model.</param>
    Task<List<Company>> GetCompaniesAsync();
}
