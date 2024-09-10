using System.IO;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.WebApi.Dtos.Preview;
using Aurigma.DirectMail.Sample.WebApi.Dtos.Response;
using Aurigma.DirectMail.Sample.WebApi.Extensions;
using Aurigma.DirectMail.Sample.WebApi.Helpers;
using Aurigma.DirectMail.Sample.WebApi.Models.Preview;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DomainEntity = Aurigma.DirectMail.Sample.DomainEntities.Entities.Preview;
using PreviewAppModels = Aurigma.DirectMail.Sample.App.Models.Preview;

namespace Aurigma.DirectMail.Sample.WebApi.Controllers;

/// <summary>
/// Used to perform operations with personalized design previews.
/// </summary>
[ApiController]
[Route("api/direct-mail/v1/previews")]
public class PreviewController(IPreviewAppService previewAppService, IMapper mapper)
    : ControllerBase
{
    private readonly IPreviewAppService _previewAppService = previewAppService;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Returns a design info.
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <returns>The design info.</returns>
    [HttpGet("design-info", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DesignInfoDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictDto))]
    public async Task<ActionResult<DesignInfoDto>> GetDesignInfo(
        [FromQuery] DesignInfoRequestModel model
    )
    {
        try
        {
            var requestAppModel = new PreviewAppModels.DesignInfoRequestAppModel
            {
                LineItemId = model.LineItemId,
            };

            var designInfo = await _previewAppService.GetDesignInfoAsync(requestAppModel);
            var designInfoDto = _mapper.Map<DesignInfoDto>(designInfo);

            return Ok(designInfoDto);
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
    /// Returns a proof file.
    /// </summary>
    /// <param name="model">Request body.</param>
    /// <returns>Design proof file.</returns>
    [HttpPost("proof", Name = "[controller]_[action]")]
    [Produces(
        contentType: "application/octet-stream",
        additionalContentTypes: ["application/json", "text/plain"]
    )]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictDto))]
    public async Task<ActionResult<Stream>> RenderDesignProof([FromBody] ProofRequestModel model)
    {
        try
        {
            var requestAppModel = _mapper.Map<PreviewAppModels.ProofRequestAppModel>(model);
            var proofFile = await _previewAppService.RenderProofForRecipientAsync(requestAppModel);

            return File(proofFile.ToByteArray(), "image/jpeg", null);
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
    /// Returns a preview file.
    /// </summary>
    /// <param name="model">Request body.</param>
    /// <returns>The preview file.</returns>
    [HttpPost("preview", Name = "[controller]_[action]")]
    [Produces(
        contentType: "application/octet-stream",
        additionalContentTypes: ["application/json", "text/plain"]
    )]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictDto))]
    public async Task<ActionResult<Stream>> RenderDesignPreview(
        [FromBody] PreviewRequestModel model
    )
    {
        try
        {
            var requestAppModel = _mapper.Map<PreviewAppModels.PreviewRequestAppModel>(model);
            var previewFile = await _previewAppService.RenderDesignPreviewAsync(requestAppModel);

            return File(previewFile.ToByteArray(), "image/jpeg", null);
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
    /// Returns an archive with proofs.
    /// </summary>
    /// <param name="model">Request body.</param>
    /// <returns> The archive with proof files.</returns>
    [HttpPost("proofs-zip", Name = "[controller]_[action]")]
    [Produces(
        contentType: "application/octet-stream",
        additionalContentTypes: ["application/json", "text/plain"]
    )]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictDto))]
    public async Task<ActionResult> DownloadProofsArchive([FromBody] ProofsZipRequestModel model)
    {
        try
        {
            var requestAppModel = _mapper.Map<PreviewAppModels.ProofZipRequestAppModel>(model);
            var zip = await _previewAppService.GeProofsZipAsync(requestAppModel);

            return File(zip.ZipData, "application/zip", zip.FileDownloadName);
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

internal class PreviewControllerMapperProfile : Profile
{
    public PreviewControllerMapperProfile()
    {
        CreateMap<ProofRequestConfigModel, PreviewAppModels.ProofRequestConfigAppModel>();
        CreateMap<ProofRequestModel, PreviewAppModels.ProofRequestAppModel>();

        CreateMap<PreviewConfigRequestModel, PreviewAppModels.PreviewRequestConfigAppModel>();
        CreateMap<PreviewRequestModel, PreviewAppModels.PreviewRequestAppModel>();

        CreateMap<ProofsZipRequestConfigModel, PreviewAppModels.ProofZipRequestConfigAppModel>();
        CreateMap<ProofsZipRequestModel, PreviewAppModels.ProofZipRequestAppModel>();

        CreateMap<DomainEntity.DesignInfo, DesignInfoDto>();
    }
}
