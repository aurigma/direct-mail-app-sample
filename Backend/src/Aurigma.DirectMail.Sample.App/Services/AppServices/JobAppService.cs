using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Exceptions.Job;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Models.Job;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Job;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aurigma.DirectMail.Sample.App.Services.AppServices;

public class JobAppService(IJobService jobService, IMapper mapper, ILogger<JobAppService> logger)
    : IJobAppService
{
    private readonly IJobService _jobService = jobService;
    private readonly ILogger<JobAppService> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<Job>> GetJobsAsync(JobRequestAppModel model)
    {
        try
        {
            var jobs = await _jobService.GetJobsAsync(_mapper.Map<JobFilter>(model));
            LogSuccessGetting(jobs.Count(), model);
            return jobs;
        }
        catch (Exception ex)
        {
            LogErrorGetting(ex, model);
            throw;
        }
    }

    public async Task<Job> RestartProcessingJobAsync(Guid id)
    {
        try
        {
            var job = await _jobService.RestartProcessingJobAsync(id);
            LogSuccessJobRestartProcessing(job);
            return job;
        }
        catch (JobNotFoundException ex)
        {
            LogJobNotFound(ex, id);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (JobStatusInvalidException ex)
        {
            LogAnInvalidRestartProcessing(ex, id);
            throw new InvalidStateAppException("Status", ex.Status.ToString(), ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorRestartProcessing(ex, id);
            throw;
        }
    }

    public async Task<IEnumerable<JobProcessingResult>> GetJobProcessingResultsAsync(Guid id)
    {
        try
        {
            var processingResults = await _jobService.GetJobProcessingResultsAsync(id);
            LogSuccessGettingProcessingResults(processingResults.Count(), id);
            return processingResults;
        }
        catch (JobNotFoundException ex)
        {
            LogJobNotFound(ex, id);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (JobStatusInvalidException ex)
        {
            LogAnInvalidRequestProcessingResults(ex, id);
            throw new InvalidStateAppException("Status", ex.Status.ToString(), ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorGettingProcessingResults(ex, id);
            throw;
        }
    }

    #region Logging

    private void LogSuccessGetting(int jobsCount, JobRequestAppModel model)
    {
        _logger.LogDebug(
            $"Jobs was returned. jobsCount={jobsCount}. requestModel={Serialize(model)}"
        );
    }

    private void LogErrorGetting(Exception ex, JobRequestAppModel model)
    {
        _logger.LogError(ex, $"Failed to request jobs. requestModel={Serialize(model)}");
    }

    private void LogSuccessJobRestartProcessing(Job job)
    {
        _logger.LogDebug($"The job processing successfully restarted. job={Serialize(job)}");
    }

    private void LogJobNotFound(Exception ex, Guid id)
    {
        _logger.LogWarning(ex, $"The job was not found. id={id}");
    }

    private void LogAnInvalidRestartProcessing(Exception ex, Guid id)
    {
        _logger.LogWarning(ex, $"Failed to restart job processing, id={id}");
    }

    private void LogErrorRestartProcessing(Exception ex, Guid id)
    {
        _logger.LogError(ex, $"Failed to restart job processing, id={id}");
    }

    private void LogSuccessGettingProcessingResults(int resultsCount, Guid id)
    {
        _logger.LogDebug(
            $"The job processing results was returned. resultsCount={resultsCount} jobId={id}"
        );
    }

    private void LogAnInvalidRequestProcessingResults(Exception ex, Guid id)
    {
        _logger.LogWarning(ex, $"Failed to request job processing results, id={id}");
    }

    private void LogErrorGettingProcessingResults(Exception ex, Guid id)
    {
        _logger.LogError(ex, $"Failed to request job processing results, id={id}");
    }

    #endregion Logging

    private static string Serialize<T>(T source)
        where T : class
    {
        return StringHelper.Serialize(source);
    }
}

internal class JobAppServiceMapperProfile : Profile
{
    public JobAppServiceMapperProfile()
    {
        CreateMap<JobRequestAppModel, JobFilter>();
    }
}
