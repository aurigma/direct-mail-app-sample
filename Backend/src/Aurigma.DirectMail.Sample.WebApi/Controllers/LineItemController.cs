using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.WebApi.Dtos.LineItem;
using Aurigma.DirectMail.Sample.WebApi.Dtos.Response;
using Aurigma.DirectMail.Sample.WebApi.Helpers;
using Aurigma.DirectMail.Sample.WebApi.Models.LineItem;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DomainEntity = Aurigma.DirectMail.Sample.DomainEntities.Entities.LineItem;
using LineItemAppModels = Aurigma.DirectMail.Sample.App.Models.LineItem;

namespace Aurigma.DirectMail.Sample.WebApi.Controllers;

/// <summary>
/// Used to perform operations with line items.
/// </summary>
[ApiController]
[Route("api/direct-mail/v1/line-items")]
public class LineItemController(IMapper mapper, ILineItemAppService lineItemAppService)
    : ControllerBase
{
    private readonly ILineItemAppService _lineItemAppService = lineItemAppService;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Returns a list of all line items.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpGet("", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LineItemDto>))]
    public async Task<ActionResult<IEnumerable<LineItemDto>>> GetLineItems(
        [FromQuery] LineItemRequestModel model
    )
    {
        var requestAppModel = _mapper.Map<LineItemAppModels.LineItemRequestModel>(model);

        var lineItems = await _lineItemAppService.GetLineItemsAsync(requestAppModel);
        var lineItemDtos = _mapper.Map<IEnumerable<LineItemDto>>(lineItems);

        return Ok(lineItemDtos);
    }

    /// <summary>
    /// Creates a new line item.
    /// </summary>
    /// <param name="model"></param>
    /// <returns>Created line item.</returns>
    [HttpPost("", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LineItemDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LineItemDto>> CreateLineItem(
        [FromBody, Required] LineItemCreationModel model
    )
    {
        try
        {
            var creationAppModel = _mapper.Map<LineItemAppModels.LineItemCreationModel>(model);
            var lineItem = await _lineItemAppService.CreateLineItemAsync(creationAppModel);

            var lineItemDto = _mapper.Map<LineItemDto>(lineItem);
            return CreatedAtAction(nameof(CreateLineItem), lineItemDto);
        }
        catch (InvalidValueAppException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Returns a line item by ID.
    /// </summary>
    /// <param name="id">Line item id.</param>
    /// <returns>The found line item.</returns>
    [HttpGet("{id}", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LineItemDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LineItemDto>> GetLineItem([FromRoute] Guid id)
    {
        try
        {
            var lineItem = await _lineItemAppService.GetLineItemByIdAsync(id);

            var lineItemDto = _mapper.Map<LineItemDto>(lineItem);
            return Ok(lineItemDto);
        }
        catch (NotFoundAppException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Updates the line item.
    /// </summary>
    /// <param name="id">Line item ID.</param>
    /// <param name="model">Update model <see cref="LineItemUpdateModel"/></param>
    /// <returns>Updated line item DTO.</returns>
    [HttpPut("{id}", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LineItemDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictDto))]
    public async Task<ActionResult<LineItemDto>> UpdateLineItem(
        [FromRoute] Guid id,
        [FromBody, Required] LineItemUpdateModel model
    )
    {
        try
        {
            var updateAppModel = BuildUpdateAppModel(model, id);

            var lineItem = await _lineItemAppService.UpdateLineItemAsync(updateAppModel);
            var lineItemDto = _mapper.Map<LineItemDto>(lineItem);

            return Ok(model);
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
    /// Process finish line item personalization.
    /// </summary>
    /// <param name="id">Line item id.</param>
    /// <returns></returns>
    [HttpPost("{id}/finish", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictDto))]
    public async Task<ActionResult> FinishPersonalizationLineItem([FromRoute] Guid id)
    {
        try
        {
            await _lineItemAppService.FinishLineItemPersonalizationAsync(id);
            return CreatedAtAction(nameof(FinishPersonalizationLineItem), null);
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

    private LineItemAppModels.LineItemUpdateModel BuildUpdateAppModel(
        LineItemUpdateModel model,
        Guid id
    )
    {
        var updateAppModel = _mapper.Map<LineItemAppModels.LineItemUpdateModel>(model);
        updateAppModel.Id = id;

        return updateAppModel;
    }
}

internal class LineItemControllerMapperProfile : Profile
{
    public LineItemControllerMapperProfile()
    {
        // Map to application models.
        CreateMap<LineItemRequestModel, LineItemAppModels.LineItemRequestModel>();
        CreateMap<LineItemCreationModel, LineItemAppModels.LineItemCreationModel>();
        CreateMap<LineItemUpdateModel, LineItemAppModels.LineItemUpdateModel>();

        // Map domain entities to DTO.
        CreateMap<DomainEntity.LineItem, LineItemDto>();
    }
}
