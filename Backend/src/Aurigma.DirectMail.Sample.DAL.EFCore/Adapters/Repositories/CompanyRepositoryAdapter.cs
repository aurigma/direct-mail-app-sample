using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;
using Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Company;
using AutoMapper;
using DomainStorage = Aurigma.DirectMail.Sample.App.Interfaces.Storages;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Adapters.Repositories;

public class CompanyRepositoryAdapter(IMapper mapper, ICompanyRepository companyRepository)
    : DomainStorage.ICompanyRepository
{
    private readonly IMapper _mapper = mapper;
    private ICompanyRepository _repository = companyRepository;

    public async Task<Company> CreateCompanyAsync(Company company)
    {
        return _mapper.Map<Company>(
            await _repository.CreateCompanyAsync(_mapper.Map<CompanyDal>(company))
        );
    }

    public async Task<Company> DeleteCompanyAsync(Guid id)
    {
        return _mapper.Map<Company>(await _repository.DeleteCompanyAsync(id));
    }

    public async Task<List<Company>> GetAllAsReadOnlyAsync()
    {
        return _mapper.Map<List<Company>>(await _repository.GetAllAsReadOnlyAsync());
    }

    public async Task<List<Company>> GetAllAsync()
    {
        return _mapper.Map<List<Company>>(await _repository.GetAllAsync());
    }

    public async Task<Company> GetCompanyByIdAsReadOnlyAsync(Guid id)
    {
        return _mapper.Map<Company>(await _repository.GetCompanyByIdAsReadOnlyAsync(id));
    }

    public async Task<Company> GetCompanyByIdAsync(Guid id)
    {
        return _mapper.Map<Company>(await _repository.GetCompanyByIdAsync(id));
    }

    public async Task<Company> UpdateCompanyAsync(Company company)
    {
        return _mapper.Map<Company>(
            await _repository.UpdateCompanyAsync(_mapper.Map<CompanyDal>(company))
        );
    }
}

internal class CompanyRepositoryAdapterMapperProfile : Profile
{
    public CompanyRepositoryAdapterMapperProfile()
    {
        CreateMap<Company, CompanyDal>()
            .ForMember(dest => dest.CreationTime, opt => opt.Ignore())
            .ForMember(dest => dest.LastModificationTime, opt => opt.Ignore());

        CreateMap<CompanyDal, Company>();
    }
}
