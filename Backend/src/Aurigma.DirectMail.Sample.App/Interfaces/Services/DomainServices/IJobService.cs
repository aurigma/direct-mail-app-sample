using Aurigma.DirectMail.Sample.App.Exceptions.Job;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Job;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;

/// <summary>
/// Domain service for performing operations with jobs.
/// </summary>
public interface IJobService
{
    /// <summary>
    /// Creates a new job.
    /// </summary>
    /// <param name="job">Model to create a job.</param>
    /// <returns>The created job.</returns>
    Task<Job> CreateJobAsync(Job job);

    /// <summary>
    /// Returns a job by id.
    /// </summary>
    /// <param name="id">ID of the job to search for.</param>
    /// <returns>The found job.</returns>
    /// <exception cref="JobNotFoundException"> The job was not found.</exception>
    Task<Job> GetJobAsync(Guid id);

    /// <summary>
    /// Returns all jobs.
    /// </summary>
    /// <returns>All jobs.</returns>
    Task<IEnumerable<Job>> GetJobsAsync(JobFilter jobFilter = null);

    /// <summary>
    /// Updates a job.
    /// </summary>
    /// <param name="job">Model to update a job.</param>
    /// <returns>The updated job.</returns>
    /// <exception cref="JobNotFoundException"> The job was not found.</exception>
    Task<Job> UpdateJobAsync(Job job);

    /// <summary>
    /// Restarted processing job.
    /// </summary>
    /// <param name="id">Job id.</param>
    /// <returns>The restarted job.</returns>
    /// <exception cref="JobNotFoundException"> The job was not found.</exception>
    /// <exception cref="JobStatusInvalidException"> The job status has an invalid value.</exception>
    Task<Job> RestartProcessingJobAsync(Guid id);

    /// <summary>
    /// Returns a collection of job processing results.
    /// </summary>
    /// <param name="id">Job id.</param>
    /// <returns>The collection of job processing results.</returns>
    /// <exception cref="JobNotFoundException"> The job was not found.</exception>
    /// <exception cref="JobStatusInvalidException"> The job status has an invalid value..</exception>
    Task<IEnumerable<JobProcessingResult>> GetJobProcessingResultsAsync(Guid id);
}
