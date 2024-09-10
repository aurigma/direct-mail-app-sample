using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Exceptions.Company;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Models.Company;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Company;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aurigma.DirectMail.Sample.App.Services.AppServices;

public class CompanyAppService(
    ILogger<CompanyAppService> logger,
    ICompanyService companyService,
    IMapper mapper
) : ICompanyAppService
{
    private readonly ICompanyService _companyService = companyService;
    private readonly ILogger<CompanyAppService> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<Company> CreateCompanyAsync(CompanyCreationModel model)
    {
        try
        {
            var company = await _companyService.CreateCompanyAsync(_mapper.Map<Company>(model));
            LogSuccessCreation(company);
            return company;
        }
        catch (Exception ex)
        {
            LogErrorCreation(ex, model);
            throw;
        }
    }

    public async Task<List<Company>> GetCompaniesAsync()
    {
        try
        {
            var companies = await _companyService.GetCompaniesAsync();
            LogSuccessGetting(companies.Count);
            return companies;
        }
        catch (Exception ex)
        {
            LogErrorGetting(ex);
            throw;
        }
    }

    public async Task<Company> GetCompanyAsync(Guid id)
    {
        try
        {
            var company = await _companyService.GetCompanyAsync(id);
            LogSuccessGettingById(company);

            return company;
        }
        catch (CompanyNotFoundException ex)
        {
            LogGettingNotFound(ex, id);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorGettingById(ex, id);
            throw;
        }
    }

    #region Logging
    private void LogSuccessCreation(Company company)
    {
        _logger.LogDebug(
            $"The company was created. Id: {company.Id}, company={Serialize(company)}"
        );
    }

    private void LogErrorCreation(Exception ex, CompanyCreationModel model)
    {
        _logger.LogError(ex, $"Failed to create a company. Request model={Serialize(model)}");
    }

    private void LogSuccessGetting(int companiesCount)
    {
        _logger.LogDebug($" Companies was returned. Companies count={companiesCount}");
    }

    private void LogErrorGetting(Exception ex)
    {
        _logger.LogError(ex, $"Failed to request a companies.");
    }

    private void LogSuccessGettingById(Company model)
    {
        _logger.LogDebug(
            $"company was returned by ID. Entity id ={model.Id}. company={Serialize(model)}"
        );
    }

    private void LogErrorGettingById(Exception ex, Guid id)
    {
        _logger.LogError(ex, $"Failed to request a company by ID. Entity id ={id}");
    }

    private void LogGettingNotFound(Exception ex, Guid id)
    {
        _logger.LogError(ex, $"The company was not found. id={id}");
    }

    #endregion Logging

    private static string Serialize<T>(T source)
        where T : class
    {
        return StringHelper.Serialize(source);
    }
}

internal class CompanyAppServiceMapperProfile : Profile
{
    public CompanyAppServiceMapperProfile()
    {
        CreateMap<CompanyCreationModel, Company>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}
