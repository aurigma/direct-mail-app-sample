using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;

public interface IJobRepository
{
    Task<JobDal> CreateJobAsync(JobDal job);
    Task<IEnumerable<JobDal>> GetAllAsReadOnlyAsync();
    Task<IEnumerable<JobDal>> GetAllAsync();
    Task<IEnumerable<JobDal>> GetJobsByFilterAsync(JobFilter jobFilter);
    Task<JobDal> GetJobByIdAsReadOnlyAsync(Guid id);
    Task<JobDal> GetJobByIdAsync(Guid id);
    Task<JobDal> UpdateJobAsync(JobDal job);
}
