using Aurigma.DirectMail.Sample.App.Exceptions.Company;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Storages;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Company;

namespace Aurigma.DirectMail.Sample.App.Services.DomainServices;

public class CompanyService(ICompanyRepository companyRepository) : ICompanyService
{
    private readonly ICompanyRepository _repository = companyRepository;

    public async Task<Company> CreateCompanyAsync(Company company)
    {
        return await _repository.CreateCompanyAsync(company);
    }

    public async Task<List<Company>> GetCompaniesAsync()
    {
        return await _repository.GetAllAsReadOnlyAsync();
    }

    public async Task<Company> GetCompanyAsync(Guid id)
    {
        return await _repository.GetCompanyByIdAsReadOnlyAsync(id)
            ?? throw new CompanyNotFoundException(
                id,
                $"The company with identifier '{id}' was not found"
            );
    }
}
