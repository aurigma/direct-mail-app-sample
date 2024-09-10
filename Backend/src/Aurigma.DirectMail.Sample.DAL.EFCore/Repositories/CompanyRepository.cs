using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;
using Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Repositories;

public class CompanyRepository : BaseRepository, ICompanyRepository
{
    private readonly DbSet<CompanyDal> _companies;
    private readonly IMapper _mapper;

    public CompanyRepository(DbContext context, IMapper mapper)
        : base(context)
    {
        _companies = DataContext.Set<CompanyDal>();
        _mapper = mapper;
    }

    public async Task<CompanyDal> CreateCompanyAsync(CompanyDal company)
    {
        PrepareForCreation(company);
        var createdEntity = (await _companies.AddAsync(company)).Entity;
        await DataContext.SaveChangesAsync();

        return createdEntity;
    }

    public async Task<CompanyDal> DeleteCompanyAsync(Guid id)
    {
        var deletingEntity = await _companies.FirstOrDefaultAsync(c => c.Id == id);

        if (deletingEntity == null)
            return null;

        _companies.Remove(deletingEntity);
        await DataContext.SaveChangesAsync();

        return deletingEntity;
    }

    public Task<IEnumerable<CompanyDal>> GetAllAsReadOnlyAsync()
    {
        return Task.FromResult<IEnumerable<CompanyDal>>(_companies.AsNoTracking());
    }

    public Task<IEnumerable<CompanyDal>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<CompanyDal>>(_companies);
    }

    public async Task<CompanyDal> GetCompanyByIdAsReadOnlyAsync(Guid id)
    {
        return await _companies.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<CompanyDal> GetCompanyByIdAsync(Guid id)
    {
        return await _companies.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<CompanyDal> UpdateCompanyAsync(CompanyDal company)
    {
        var dalEntity = await _companies.FirstOrDefaultAsync(c => c.Id == company.Id);

        if (dalEntity == null)
            return null;

        _mapper.Map(company, dalEntity);
        PrepareForUpdate(dalEntity);

        await DataContext.SaveChangesAsync();

        return dalEntity;
    }
}

internal class CompanyRepositoryMapperProfile : Profile
{
    public CompanyRepositoryMapperProfile()
    {
        CreateMap<CompanyDal, CompanyDal>();
    }
}
