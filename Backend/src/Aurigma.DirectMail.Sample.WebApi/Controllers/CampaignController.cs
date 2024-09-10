using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.WebApi.Dtos.Campaign;
using Aurigma.DirectMail.Sample.WebApi.Dtos.Category;
using Aurigma.DirectMail.Sample.WebApi.Dtos.LineItem;
using Aurigma.DirectMail.Sample.WebApi.Dtos.Product;
using Aurigma.DirectMail.Sample.WebApi.Enums;
using Aurigma.DirectMail.Sample.WebApi.Models.Campaign;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CampaignAppModels = Aurigma.DirectMail.Sample.App.Models.Campaign;
using DomainEntity = Aurigma.DirectMail.Sample.DomainEntities.Entities;
using DomainEnum = Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.WebApi.Controllers;

/// <summary>
/// Used to perform operations with campaigns.
/// </summary>
/// <param name="campaignService"></param>
/// <param name="mapper"></param>
[ApiController]
[Route("api/direct-mail/v1/campaigns")]
public class CampaignController(ICampaignAppService campaignService, IMapper mapper)
    : ControllerBase
{
    private readonly ICampaignAppService _campaignAppService = campaignService;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Returns a list of all campaigns.
    /// </summary>
    /// <param name="model">Request filter <see cref="CampaignRequestModel"/></param>
    /// <returns>List of campaigns.</returns>
    [HttpGet("", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CampaignDto>))]
    public async Task<ActionResult<IEnumerable<CampaignDto>>> GetCampaigns(
        [FromQuery] CampaignRequestModel model
    )
    {
        var requestModel = _mapper.Map<CampaignAppModels.CampaignRequestModel>(model);

        var campaigns = await _campaignAppService.GetCampaignsAsync(requestModel);
        var campaignDtos = _mapper.Map<List<CampaignDto>>(campaigns);

        return Ok(campaignDtos);
    }

    /// <summary>
    /// Creates a new campaign.
    /// </summary>
    /// <param name="model">Creation model <see cref="CampaignCreationModel"/></param>
    /// <returns>Created campaign.</returns>
    [HttpPost("", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CampaignDto))]
    public async Task<ActionResult<CampaignDto>> CreateCampaign(
        [FromBody, Required] CampaignCreationModel model
    )
    {
        var appCreationModel = _mapper.Map<CampaignAppModels.CampaignCreationModel>(model);

        var createdCampaign = await _campaignAppService.CreateCampaignAsync(appCreationModel);
        var campaignDto = _mapper.Map<CampaignDto>(createdCampaign);

        return CreatedAtAction(nameof(CreateCampaign), campaignDto);
    }

    /// <summary>
    /// Returns a campaign by id.
    /// </summary>
    /// <param name="id">Campaign's id.</param>
    /// <returns>The found category.</returns>
    [HttpGet("{id}", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CampaignDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CampaignDto>> GetCampaignById([FromRoute] Guid id)
    {
        try
        {
            var campaign = await _campaignAppService.GetCampaignById(id);
            var campaignDto = _mapper.Map<CampaignDto>(campaign);

            return Ok(campaignDto);
        }
        catch (NotFoundAppException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Updates a campaign
    /// </summary>
    /// <param name="id">Campaign's id.</param>
    /// <param name="model">Update model.</param>
    /// <returns>The updated campaign.</returns>
    [HttpPut("{id}", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CampaignDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CampaignDto>> UpdateCampaign(
        [FromRoute] Guid id,
        [FromBody] CampaignUpdateModel model
    )
    {
        try
        {
            var updateAppModel = BuildUpdateAppModel(model, id);
            var campaign = await _campaignAppService.UpdateCampaignAsync(updateAppModel);
            var campaignDto = _mapper.Map<CampaignDto>(campaign);
            return Ok(campaignDto);
        }
        catch (InvalidValueAppException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundAppException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Returns available campaign types.
    /// </summary>
    /// <returns></returns>
    [HttpGet("types", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CampaignType>))]
    public ActionResult<IEnumerable<CampaignType>> GetCampaignTypes()
    {
        var campaignTypes = _campaignAppService.GetCampaignTypes();
        var campaignTypesDto = _mapper.Map<IEnumerable<CampaignType>>(campaignTypes);

        return Ok(campaignTypesDto);
    }

    private CampaignAppModels.CampaignUpdateModel BuildUpdateAppModel(
        CampaignUpdateModel model,
        Guid id
    )
    {
        var updateAppModel = _mapper.Map<CampaignAppModels.CampaignUpdateModel>(model);
        updateAppModel.Id = id;

        return updateAppModel;
    }
}

internal class CampaignControllerMapperProfile : Profile
{
    public CampaignControllerMapperProfile()
    {
        // Map enums.
        CreateMap<DomainEnum.CampaignType, CampaignType>()
            .ReverseMap();

        CreateMap<DomainEntity.Category.Category, CategoryDto>();
        CreateMap<DomainEntity.Product.Product, ProductDto>();
        CreateMap<DomainEntity.LineItem.LineItem, LineItemDto>();
        CreateMap<DomainEntity.Campaign.Campaign, CampaignDto>();

        CreateMap<CampaignCreationModel, CampaignAppModels.CampaignCreationModel>();
        CreateMap<CampaignRequestModel, CampaignAppModels.CampaignRequestModel>();
        CreateMap<CampaignUpdateModel, CampaignAppModels.CampaignUpdateModel>();
    }
}
