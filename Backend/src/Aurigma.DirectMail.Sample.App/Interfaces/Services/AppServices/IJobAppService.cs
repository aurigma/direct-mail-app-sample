using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Models.Job;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Job;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;

/// <summary>
/// Application service for performing operations with jobs.
/// </summary>
public interface IJobAppService
{
    /// <summary>
    /// Creates a new job.
    /// </summary>
    /// <param name="model">Model to create a job.</param>
    /// <returns>The created job.</returns>
    Task<IEnumerable<Job>> GetJobsAsync(JobRequestAppModel model);

    /// <summary>
    /// Restarted processing job.
    /// </summary>
    /// <param name="id">Job id.</param>
    /// <returns>The restarted job.</returns>
    /// <exception cref="NotFoundAppException"> The job was not found.</exception>
    /// <exception cref="InvalidStateAppException"> The property has an invalid value.</exception>
    Task<Job> RestartProcessingJobAsync(Guid id);

    /// <summary>
    /// Returns a collection of job processing results.
    /// </summary>
    /// <param name="id">Job id.</param>
    /// <returns>The collection of job processing results.</returns>
    /// <exception cref="NotFoundAppException"> The job was not found.</exception>
    /// <exception cref="InvalidStateAppException"> The property has an invalid value.</exception>
    Task<IEnumerable<JobProcessingResult>> GetJobProcessingResultsAsync(Guid id);
}
