using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.WebApi.Dtos.Company;
using Aurigma.DirectMail.Sample.WebApi.Models.Company;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CompanyAppModels = Aurigma.DirectMail.Sample.App.Models.Company;
using DomainEntity = Aurigma.DirectMail.Sample.DomainEntities.Entities.Company;

namespace Aurigma.DirectMail.Sample.WebApi.Controllers;

[ApiController]
[Route("api/direct-mail/v1/companies")]
public class CompanyController(ICompanyAppService companyAppService, IMapper mapper)
    : ControllerBase
{
    private readonly ICompanyAppService _companyAppService = companyAppService;
    private readonly IMapper _mapper = mapper;

    [HttpGet("", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CompanyDto>))]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies()
    {
        var companies = await _companyAppService.GetCompaniesAsync();

        var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        return Ok(companyDtos);
    }

    [HttpPost("", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CompanyDto))]
    public async Task<ActionResult<CompanyDto>> CreateCompany(
        [FromBody, Required] CompanyCreationModel model
    )
    {
        var creationAppModel = new CompanyAppModels.CompanyCreationModel() { Name = model.Name };

        var company = await _companyAppService.CreateCompanyAsync(creationAppModel);
        var companyDto = _mapper.Map<CompanyDto>(company);

        return CreatedAtAction(nameof(CreateCompany), companyDto);
    }

    [HttpGet("{id}", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CompanyDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CompanyDto>> GetCompany([FromRoute] Guid id)
    {
        try
        {
            var company = await _companyAppService.GetCompanyAsync(id);
            var companyDto = _mapper.Map<CompanyDto>(company);

            return Ok(companyDto);
        }
        catch (NotFoundAppException)
        {
            return NotFound();
        }
    }
}

internal class CompanyControllerMapperProfile : Profile
{
    public CompanyControllerMapperProfile()
    {
        CreateMap<DomainEntity.Company, CompanyDto>();
    }
}
