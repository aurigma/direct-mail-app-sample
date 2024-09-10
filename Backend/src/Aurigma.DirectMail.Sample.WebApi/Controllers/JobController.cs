using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.WebApi.Dtos.Job;
using Aurigma.DirectMail.Sample.WebApi.Dtos.Response;
using Aurigma.DirectMail.Sample.WebApi.Enums;
using Aurigma.DirectMail.Sample.WebApi.Helpers;
using Aurigma.DirectMail.Sample.WebApi.Models.Job;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DomainEntity = Aurigma.DirectMail.Sample.DomainEntities.Entities.Job;
using JobAppModels = Aurigma.DirectMail.Sample.App.Models.Job;

namespace Aurigma.DirectMail.Sample.WebApi.Controllers;

/// <summary>
/// Used to perform operations with jobs.
/// </summary>
/// <param name="jobAppService"></param>
/// <param name="mapper"></param>
[ApiController]
[Route("api/direct-mail/v1/jobs")]
public class JobController(IJobAppService jobAppService, IMapper mapper) : ControllerBase
{
    private readonly IJobAppService _jobAppService = jobAppService;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Returns a list of jobs.
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <returns>The list of jobs.</returns>
    [HttpGet("", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<JobDto>))]
    public async Task<ActionResult<IEnumerable<JobDto>>> GetJobs([FromQuery] JobRequestModel model)
    {
        var jobs = await _jobAppService.GetJobsAsync(
            _mapper.Map<JobAppModels.JobRequestAppModel>(model)
        );
        var jobDtos = _mapper.Map<IEnumerable<JobDto>>(jobs);

        return Ok(jobDtos);
    }

    /// <summary>
    /// Restarts a job.
    /// </summary>
    /// <param name="id">Job id.</param>
    /// <returns>Restated job dto.</returns>
    [HttpPost("{id}/restart")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictDto))]
    public async Task<ActionResult<JobDto>> RestartJob([FromRoute] Guid id)
    {
        try
        {
            var restartedJob = await _jobAppService.RestartProcessingJobAsync(id);
            var restartedJobDto = _mapper.Map<JobDto>(restartedJob);
            return Ok(restartedJobDto);
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
    /// Returns a collection of processing results.
    /// </summary>
    /// <param name="id">Job id.</param>
    /// <returns>The  collection of processing results.</returns>
    [HttpPost("{id}/download-results")]
    [ProducesResponseType(
        StatusCodes.Status200OK,
        Type = typeof(IEnumerable<JobProcessingResultDto>)
    )]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ConflictDto))]
    public async Task<ActionResult<IEnumerable<JobProcessingResultDto>>> DownloadJobResults(
        [FromRoute] Guid id
    )
    {
        try
        {
            var processingResults = await _jobAppService.GetJobProcessingResultsAsync(id);
            var processingResultDtos = _mapper.Map<IEnumerable<JobProcessingResultDto>>(
                processingResults
            );
            return Ok(processingResultDtos);
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

internal class JobControllerMapperProfile : Profile
{
    public JobControllerMapperProfile()
    {
        CreateMap<DomainEntities.Enums.JobStatus, JobStatus>();
        CreateMap<JobStatus, DomainEntities.Enums.JobStatus>();

        CreateMap<JobRequestModel, JobAppModels.JobRequestAppModel>();
        CreateMap<DomainEntity.Job, JobDto>();

        CreateMap<DomainEntity.JobProcessingResult, JobProcessingResultDto>();
    }
}
