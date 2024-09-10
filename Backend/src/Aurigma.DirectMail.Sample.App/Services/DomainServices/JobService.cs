using Aurigma.DirectMail.Sample.App.Exceptions.Job;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Storages;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Job;
using Aurigma.DirectMail.Sample.DomainEntities.Enums;
using AutoMapper;

namespace Aurigma.DirectMail.Sample.App.Services.DomainServices;

public class JobService(
    IJobRepository jobRepository,
    IMapper mapper,
    IProjectService projectService
) : IJobService
{
    private readonly IJobRepository _jobRepository = jobRepository;
    private readonly IProjectService _projectService = projectService;
    private readonly IMapper _mapper = mapper;

    public async Task<Job> CreateJobAsync(Job job)
    {
        return await _jobRepository.CreateJobAsync(job);
    }

    public async Task<IEnumerable<Job>> GetJobsAsync(JobFilter jobFilter = null)
    {
        var jobs = new List<Job>();
        if (jobFilter is null)
            jobs = await _jobRepository.GetAllAsReadOnlyAsync();

        jobs = await _jobRepository.GetJobsByFilterAsync(jobFilter);

        if (jobs.Any())
            await ActualizeJobsStatuses(jobs);

        return jobs;
    }

    public async Task<Job> GetJobAsync(Guid id)
    {
        return await _jobRepository.GetJobByIdAsReadOnlyAsync(id)
            ?? throw new JobNotFoundException(id, $"The job with identifier {id} was not found");
    }

    public async Task<Job> UpdateJobAsync(Job job)
    {
        var dbJob =
            await _jobRepository.GetJobByIdAsReadOnlyAsync(job.Id)
            ?? throw new JobNotFoundException(
                job.Id,
                $"The job with identifier {job.Id} was not found"
            );

        _mapper.Map(job, dbJob);

        return await _jobRepository.UpdateJobAsync(dbJob);
    }

    public async Task<Job> RestartProcessingJobAsync(Guid id)
    {
        var job = await GetJobAsync(id);

        if (job.Status is not JobStatus.Failed)
            throw new JobStatusInvalidException(
                id,
                job.Status,
                $"The job with identifier {id} has a status that is not available for restart"
            );

        await _projectService.RestartProcessingAsync(job.CustomersCanvasProjectId);

        job.Status = JobStatus.Pending;
        var updatedJob = await _jobRepository.UpdateJobAsync(job);

        return updatedJob;
    }

    public async Task<IEnumerable<JobProcessingResult>> GetJobProcessingResultsAsync(Guid id)
    {
        var job = await GetJobAsync(id);

        if (job.Status is not JobStatus.Completed)
            throw new JobStatusInvalidException(
                id,
                job.Status,
                $"The job with identifier {id} has a status that is not available for download results"
            );

        var processingResults = await _projectService.GetProcessingResultsAsync(
            new Models.Project.ProcessingResultsRequestAppModel
            {
                ProjectId = job.CustomersCanvasProjectId,
            }
        );

        return processingResults.OutputFileDetails.Select(f => new JobProcessingResult
        {
            Url = f.Url,
            Format = f.Format,
            Name = f.Name,
        });
    }

    private async Task<IEnumerable<Job>> ActualizeJobsStatuses(List<Job> jobs)
    {
        foreach (var job in jobs)
        {
            var processingResults = await _projectService.GetProcessingResultsAsync(
                new Models.Project.ProcessingResultsRequestAppModel
                {
                    ProjectId = job.CustomersCanvasProjectId,
                }
            );
            if ((int)job.Status != (int)processingResults.Status)
            {
                job.Status = _mapper.Map<JobStatus>(processingResults.Status);
                await UpdateJobAsync(job);
            }
        }

        return jobs;
    }
}

internal class JobServiceMapperProfile : Profile
{
    public JobServiceMapperProfile()
    {
        CreateMap<StorefrontApi.ProjectProcessingStatus, JobStatus>();
        CreateMap<Job, Job>();
    }
}
