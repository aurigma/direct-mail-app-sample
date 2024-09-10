using Aurigma.DirectMail.Sample.DomainEntities.Entities.Company;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Storages;

public interface ICompanyRepository
{
    Task<Company> GetCompanyByIdAsync(Guid id);
    Task<Company> GetCompanyByIdAsReadOnlyAsync(Guid id);

    Task<List<Company>> GetAllAsync();
    Task<List<Company>> GetAllAsReadOnlyAsync();

    Task<Company> CreateCompanyAsync(Company company);
    Task<Company> UpdateCompanyAsync(Company company);
    Task<Company> DeleteCompanyAsync(Guid id);
}
