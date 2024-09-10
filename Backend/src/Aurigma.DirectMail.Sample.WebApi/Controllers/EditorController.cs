using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Design;
using Aurigma.DirectMail.Sample.WebApi.Dtos.Design;
using Aurigma.DirectMail.Sample.WebApi.Dtos.Editor;
using Aurigma.DirectMail.Sample.WebApi.Dtos.Response;
using Aurigma.DirectMail.Sample.WebApi.Enums;
using Aurigma.DirectMail.Sample.WebApi.Helpers;
using Aurigma.DirectMail.Sample.WebApi.Models.Editor;
using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DomainEntites = Aurigma.DirectMail.Sample.DomainEntities.Entities.Editor;
using DomainEnums = Aurigma.DirectMail.Sample.DomainEntities.Enums;

namespace Aurigma.DirectMail.Sample.WebApi.Controllers;

/// <summary>
/// Used to perform operations with Design Editor.
/// </summary>
[ApiController]
[Route("api/direct-mail/v1/editor")]
public class EditorController(IEditorAppService editorAppService, IMapper mapper) : ControllerBase
{
    private readonly IEditorAppService _editorAppService = editorAppService;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Returns a integration details data necessary to initialize the editor.
    /// </summary>
    /// <param name="referenceId">Product reference id.</param>
    /// <returns></returns>
    [HttpGet("integration-details", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IntegrationDetailsDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IntegrationDetailsDto>> GetIntegrationDetails(
        [FromQuery] string referenceId
    )
    {
        try
        {
            var editorInfo = await _editorAppService.GetIntegrationDetailsAsync(referenceId);
            var editorInfoDto = _mapper.Map<IntegrationDetailsDto>(editorInfo);

            return Ok(editorInfoDto);
        }
        catch (NotFoundAppException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Returns a token for Design Editor.
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <returns>Design Editor token.</returns>
    [HttpPost("token", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDto))]
    public async Task<ActionResult<TokenDto>> GetEditorToken([FromBody] TokenRequestModel model)
    {
        var token = await _editorAppService.GetEditorTokenAsync(model.UserId);
        var tokenDto = _mapper.Map<TokenDto>(token);
        return Ok(tokenDto);
    }

    /// <summary>
    /// Validates a design for the presence of VDP variables.
    /// </summary>
    /// <param name="lineItemId">Current line item id.</param>
    /// <returns>Validation result.</returns>
    [HttpPost("validate-design", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DesignValidationResultDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictDto))]
    public async Task<ActionResult<DesignValidationResultDto>> ValidateDesign(
        [FromQuery] Guid lineItemId
    )
    {
        try
        {
            var validationResult = await _editorAppService.ValidateDesignAsync(lineItemId);
            var validationResultDto = _mapper.Map<DesignValidationResultDto>(validationResult);

            return Ok(validationResultDto);
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
    /// Returns a collection of available variables.
    /// </summary>
    /// <param name="lineItemId">Current line item id.</param>
    /// <returns>The collection of variables.</returns>
    [HttpGet("available-variables", Name = "[controller]_[action]")]
    [ProducesResponseType(
        StatusCodes.Status200OK,
        Type = typeof(IEnumerable<EditorVariableInfoDto>)
    )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<EditorVariableInfoDto>>> GetAvailableVariables(
        [FromQuery] Guid lineItemId
    )
    {
        try
        {
            var availableVariables = await _editorAppService.GetAvailableVariablesAsync(lineItemId);
            var availableVariableDtos = _mapper.Map<IEnumerable<EditorVariableInfoDto>>(
                availableVariables
            );

            return Ok(availableVariableDtos);
        }
        catch (NotFoundAppException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Creates a design for personalization in the Design Editor.
    /// </summary>
    /// <param name="lineItemId">Current line item id.</param>
    /// <param name="userId">Storefront user id.</param>
    /// <returns>The design for personalization.</returns>
    /// <remarks>If a resource of type EditorModel is attached to a design-variant, it generates a design from it. If not, then from public design.</remarks>
    [HttpPost("design", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DesignDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictDto))]
    public async Task<ActionResult<DesignDto>> CreateEditorDesign(
        [FromQuery, Required] Guid lineItemId,
        [FromQuery, Required] string userId
    )
    {
        try
        {
            var design = await _editorAppService.CreateEditorDesignAsync(lineItemId, userId);
            var designDto = _mapper.Map<DesignDto>(design);
            return Ok(designDto);
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

internal class EditorInfoControllerMapperProfile : Profile
{
    public EditorInfoControllerMapperProfile()
    {
        CreateMap<DomainEntites.IntegrationDetails, IntegrationDetailsDto>();
        CreateMap<DomainEntites.Token, TokenDto>();
        CreateMap<DomainEntites.DesignValidationResult, DesignValidationResultDto>();

        CreateMap<DomainEnums.EditorVariableInfoType, EditorVariableInfoType>()
            .ConvertUsingEnumMapping(opt => opt.MapByName());
        CreateMap<DomainEnums.EditorVariableInfoBarcodeFormat, EditorVariableInfoBarcodeFormat>()
            .ConvertUsingEnumMapping(opt => opt.MapByName());
        CreateMap<DomainEnums.EditorVariableInfoBarcodeSubType, EditorVariableInfoBarcodeSubType>()
            .ConvertUsingEnumMapping(opt => opt.MapByName());
        CreateMap<DomainEntites.EditorVariableInfo, EditorVariableInfoDto>();

        CreateMap<Design, DesignDto>();
    }
}
