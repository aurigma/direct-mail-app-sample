using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.WebApi.Dtos.IntegratedProduct;
using Aurigma.DirectMail.Sample.WebApi.Dtos.Response;
using Aurigma.DirectMail.Sample.WebApi.Enums;
using Aurigma.DirectMail.Sample.WebApi.Helpers;
using Aurigma.DirectMail.Sample.WebApi.Models.IntegratedProduct;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DomainEntity = Aurigma.DirectMail.Sample.DomainEntities.Entities.IntegratedProduct;
using DomainEnum = Aurigma.DirectMail.Sample.DomainEntities.Enums;
using IntegratedProductAppModels = Aurigma.DirectMail.Sample.App.Models.IntegratedProduct;

namespace Aurigma.DirectMail.Sample.WebApi.Controllers;

/// <summary>
/// Used to perform operations on products that are integrated into Customer's Canvas.
/// </summary>
[ApiController]
[Route("api/direct-mail/v1/integrated-products")]
public class IntegratedProductController(
    IIntegratedProductAppService integratedProductAppService,
    IMapper mapper
) : ControllerBase
{
    private readonly IIntegratedProductAppService _integratedProductAppService =
        integratedProductAppService;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Returns a list of all integrated products.
    /// </summary>
    /// <param name="model">Request filter <see cref="IntegratedProductRequestModel"/></param>
    /// <returns>List of integrated products.</returns>
    [HttpGet("", Name = "[controller]_[action]")]
    [ProducesResponseType(
        StatusCodes.Status200OK,
        Type = typeof(IEnumerable<IntegratedProductDto>)
    )]
    public async Task<ActionResult<IEnumerable<IntegratedProductDto>>> GetIntegratedProducts(
        [FromQuery] IntegratedProductRequestModel model
    )
    {
        var requestAppModel = new IntegratedProductAppModels.IntegratedProductRequestModel()
        {
            CategoryId = model.CategoryId ?? null,
        };

        var integratedProducts = await _integratedProductAppService.GetIntegratedProductsAsync(
            requestAppModel
        );

        var integratedProductDtos = _mapper.Map<IEnumerable<IntegratedProductDto>>(
            integratedProducts
        );
        return Ok(integratedProductDtos);
    }

    /// <summary>
    /// Updates a integrated product resources.
    /// </summary>
    /// <param name="id">Product identifier.</param>
    [HttpPost("{id}/update-resources", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictDto))]
    public async Task<ActionResult> UpdateIntegratedProductResources([FromRoute] Guid id)
    {
        try
        {
            await _integratedProductAppService.UpdateIntegratedProductResourcesAsync(id);
            return Ok();
        }
        catch (NotFoundAppException)
        {
            return NotFound();
        }
        catch (InvalidStateAppException ex)
        {
            return Conflict(ResponseHelper.BuildConflictDto(ex));
        }
    }

    /// <summary>
    /// Returns a list of all integrated product options.
    /// </summary>
    /// <param name="id">Product id.</param>
    /// <returns>List of integrated product options.</returns>
    [HttpGet("{id}/options", Name = "[controller]_[action]")]
    [ProducesResponseType(
        StatusCodes.Status200OK,
        Type = typeof(IEnumerable<IntegratedProductOptionDto>)
    )]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictDto))]
    public async Task<
        ActionResult<IEnumerable<IntegratedProductOptionDto>>
    > GetIntegratedProductOptions([FromRoute] Guid id)
    {
        try
        {
            var integratedProductOptions =
                await _integratedProductAppService.GetIntegratedProductOptionsAsync(id);

            var integratedProductOptionsDtos = _mapper.Map<IEnumerable<IntegratedProductOptionDto>>(
                integratedProductOptions
            );

            return Ok(integratedProductOptionsDtos);
        }
        catch (InvalidValueAppException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundAppException)
        {
            return NotFound();
        }
        catch (InvalidStateAppException ex)
        {
            return Conflict(ResponseHelper.BuildConflictDto(ex));
        }
    }

    /// <summary>
    /// Returns a list of all integrated product templates.
    /// </summary>
    /// <param name="id">Product id.</param>
    /// <param name="model">Defines options filter.</param>
    /// <returns>List of integrated product templates.</returns>
    [HttpPost("{id}/templates", Name = "[controller]_[action]")]
    [ProducesResponseType(
        StatusCodes.Status200OK,
        Type = typeof(IEnumerable<IntegratedProductTemplateDto>)
    )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictDto))]
    public async Task<
        ActionResult<IEnumerable<IntegratedProductTemplateDto>>
    > GetIntegratedProductTemplates(
        [FromRoute, Required] Guid id,
        [FromBody] IntegrationProductOptionRequestModel model
    )
    {
        try
        {
            var requestAppModel =
                _mapper.Map<App.Models.IntegratedProduct.IntegrationProductOptionRequestModel>(
                    model
                );
            var integratedProductTemplates =
                await _integratedProductAppService.GetIntegratedProductTemplatesAsync(
                    id,
                    requestAppModel
                );

            var integratedProductTemplateDtos = _mapper.Map<
                IEnumerable<IntegratedProductTemplateDto>
            >(integratedProductTemplates);
            return Ok(integratedProductTemplateDtos);
        }
        catch (NotFoundAppException)
        {
            return NotFound();
        }
        catch (InvalidStateAppException ex)
        {
            return Conflict(ResponseHelper.BuildConflictDto(ex));
        }
    }

    /// <summary>
    /// Returns a integrated product template details.
    /// </summary>
    /// <param name="id"> Product id.</param>
    /// <param name="templateId"> Template id.</param>
    /// <param name="model">Defines options filter.</param>
    /// <returns> The integrated product template details.</returns>
    [HttpGet("{id}/templates/{templateId}", Name = "[controller]_[action]")]
    [ProducesResponseType(
        StatusCodes.Status200OK,
        Type = typeof(IntegratedProductTemplateDetailsDto)
    )]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictDto))]
    public async Task<
        ActionResult<IntegratedProductTemplateDetailsDto>
    > GetIntegratedProductTemplateDetails(
        [FromRoute] Guid id,
        string templateId,
        [FromQuery, Required] IntegratedProductTemplateDetailsRequestModel model
    )
    {
        try
        {
            var requestAppModel =
                _mapper.Map<App.Models.IntegratedProduct.IntegratedProductTemplateDetailsRequestModel>(
                    model
                );
            requestAppModel.TemplateId = templateId;

            var templateDetails =
                await _integratedProductAppService.GetIntegratedProductTemplateDetailsAsync(
                    id,
                    requestAppModel
                );
            var templateDetailsDto = _mapper.Map<IntegratedProductTemplateDetailsDto>(
                templateDetails
            );

            return Ok(templateDetailsDto);
        }
        catch (InvalidValueAppException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundAppException)
        {
            return NotFound();
        }
        catch (InvalidStateAppException ex)
        {
            return Conflict(ResponseHelper.BuildConflictDto(ex));
        }
    }
}

internal class IntegratedProductControllerMapperProfile : Profile
{
    public IntegratedProductControllerMapperProfile()
    {
        CreateMap<DomainEntity.IntegratedProduct, IntegratedProductDto>();

        CreateMap<DomainEnum.IntegratedProductOptionType, IntegratedProductOptionType>()
            .ReverseMap();
        CreateMap<DomainEntity.IntegratedProductOptionValue, IntegratedProductOptionValueDto>();
        CreateMap<DomainEntity.IntegratedProductOption, IntegratedProductOptionDto>();

        CreateMap<
            IntegrationProductOptionItemRequestModel,
            IntegratedProductAppModels.IntegrationProductOptionItemRequestModel
        >();
        CreateMap<
            IntegrationProductOptionRequestModel,
            IntegratedProductAppModels.IntegrationProductOptionRequestModel
        >();
        CreateMap<
            IntegratedProductTemplateDetailsRequestModel,
            IntegratedProductAppModels.IntegratedProductTemplateDetailsRequestModel
        >();

        CreateMap<DomainEntity.IntegratedProductTemplate, IntegratedProductTemplateDto>();
        CreateMap<
            DomainEntity.IntegratedProductTemplateDetailsOption,
            IntegratedProductTemplateDetailsOptionDto
        >();
        CreateMap<
            DomainEntity.IntegratedProductTemplateDetails,
            IntegratedProductTemplateDetailsDto
        >();
    }
}
