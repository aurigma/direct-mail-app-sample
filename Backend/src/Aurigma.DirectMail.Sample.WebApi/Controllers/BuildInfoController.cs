using System.Reflection;
using Aurigma.DirectMail.Sample.WebApi.Attributes;
using Aurigma.DirectMail.Sample.WebApi.Models.BuildInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Aurigma.DirectMail.Sample.WebApi.Controllers;

/// <summary>
/// Provides information about the application, such as version, build date, etc.
/// </summary>
[ApiController]
[AllowAnonymous]
[EnableCors("AllowAny")]
public class BuildInfoController : ControllerBase
{
    /// <summary>
    /// Returns build info.
    /// </summary>
    [HttpHead("api/direct-mail/v1/info", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseHeader(
        "X-Aurigma-Version",
        StatusCodes.Status200OK,
        Description = "A version number"
    )]
    [ProducesResponseHeader(
        "X-Aurigma-Build-Date",
        StatusCodes.Status200OK,
        Description = "A build date (UTC)"
    )]
    [ProducesResponseHeader(
        "X-Aurigma-Configuration",
        StatusCodes.Status200OK,
        Description = "A build configuration (Debug | Release)"
    )]
    [ProducesResponseHeader(
        "X-Aurigma-App-Name",
        StatusCodes.Status200OK,
        Description = "A service name"
    )]
    public ActionResult HeadInfo()
    {
        var buildInfo = GetBuildInfo();

        Response.Headers.Append("X-Aurigma-Version", buildInfo.Version);
        Response.Headers.Append("X-Aurigma-Build-Date", buildInfo.BuildDate);
        Response.Headers.Append("X-Aurigma-Configuration", buildInfo.Configuration);
        Response.Headers.Append("X-Aurigma-App-Name", buildInfo.AppName);

        return Ok();
    }

    /// <summary>
    /// Returns build info.
    /// </summary>
    [HttpGet("api/direct-mail/v1/info", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BuildInfoModel))]
    public ActionResult<BuildInfoModel> GetInfo()
    {
        var buildInfo = GetBuildInfo();

        return Ok(buildInfo);
    }

    /// <summary>
    /// Default action.
    /// </summary>
    /// <returns>Build info.</returns>
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet("", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<BuildInfoModel> Default()
    {
        return GetInfo();
    }

    private static BuildInfoModel GetBuildInfo()
    {
        var assembly = Assembly.GetExecutingAssembly();
        return new BuildInfoModel(assembly);
    }
}
