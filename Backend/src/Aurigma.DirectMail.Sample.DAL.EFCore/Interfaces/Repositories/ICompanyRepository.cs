using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;

public interface ICompanyRepository
{
    Task<CompanyDal> GetCompanyByIdAsync(Guid id);
    Task<CompanyDal> GetCompanyByIdAsReadOnlyAsync(Guid id);

    Task<IEnumerable<CompanyDal>> GetAllAsync();
    Task<IEnumerable<CompanyDal>> GetAllAsReadOnlyAsync();

    Task<CompanyDal> CreateCompanyAsync(CompanyDal company);
    Task<CompanyDal> UpdateCompanyAsync(CompanyDal company);
    Task<CompanyDal> DeleteCompanyAsync(Guid id);
}
