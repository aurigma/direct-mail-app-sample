using System.Collections.Generic;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.WebApi.Dtos.RecipientList;
using Aurigma.DirectMail.Sample.WebApi.Models.RecipientList;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DomainEntity = Aurigma.DirectMail.Sample.DomainEntities.Entities.RecipientList;
using RecipientListAppModels = Aurigma.DirectMail.Sample.App.Models.RecipientList;

namespace Aurigma.DirectMail.Sample.WebApi.Controllers;

/// <summary>
/// Used to perform operations with recipient lists.
/// </summary>
[ApiController]
[Route("api/direct-mail/v1/recipient-lists")]
public class RecipientListController(
    IRecipientListAppService recipientListAppService,
    IMapper mapper
) : ControllerBase
{
    private readonly IRecipientListAppService _recipientListAppService = recipientListAppService;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Returns a collection of all recipient lists.
    /// </summary>
    /// <returns> Collection of recipient lists.</returns>
    [HttpGet("", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RecipientListDto>))]
    public async Task<ActionResult<IEnumerable<RecipientListDto>>> GetRecipientLists()
    {
        var lists = await _recipientListAppService.GetRecipientListsAsync();
        var listDtos = _mapper.Map<IEnumerable<RecipientListDto>>(lists);
        return Ok(listDtos);
    }

    /// <summary>
    /// Submit a recipient lists.
    /// </summary>
    /// <returns></returns>
    [HttpPost("submit", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RecipientListDto>))]
    public async Task<ActionResult<IEnumerable<RecipientListDto>>> SubmitRecipientLists(
        [FromBody] RecipientListSubmitModel model
    )
    {
        var appModel = _mapper.Map<RecipientListAppModels.RecipientListSubmitAppModel>(model);

        var lists = await _recipientListAppService.SubmitRecipientListsAsync(appModel);
        var listDtos = _mapper.Map<IEnumerable<RecipientListDto>>(lists);
        return Ok(listDtos);
    }
}

internal class RecipientListControllerMapperProfile : Profile
{
    public RecipientListControllerMapperProfile()
    {
        CreateMap<DomainEntity.Recipient, RecipientDto>();
        CreateMap<DomainEntity.RecipientList, RecipientListDto>();

        CreateMap<RecipientListSubmitModel, RecipientListAppModels.RecipientListSubmitAppModel>();
    }
}
