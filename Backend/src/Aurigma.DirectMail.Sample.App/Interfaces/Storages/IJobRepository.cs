using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Job;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Storages;

public interface IJobRepository
{
    Task<Job> CreateJobAsync(Job job);
    Task<List<Job>> GetAllAsReadOnlyAsync();
    Task<Job> GetJobByIdAsReadOnlyAsync(Guid id);
    Task<List<Job>> GetJobsByFilterAsync(JobFilter jobFilter);
    Task<Job> GetJobByIdAsync(Guid id);
    Task<Job> UpdateJobAsync(Job job);
}
