using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;
using Aurigma.DirectMail.Sample.DAL.EFCore.Extensions;
using Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Repositories;

public class JobRepository : BaseRepository, IJobRepository
{
    private readonly DbSet<JobDal> _jobs;
    private readonly IMapper _mapper;

    public JobRepository(DbContext context, IMapper mapper)
        : base(context)
    {
        _jobs = DataContext.Set<JobDal>();
        _mapper = mapper;
    }

    public async Task<JobDal> CreateJobAsync(JobDal job)
    {
        PrepareForCreation(job);

        var createdEntity = (await _jobs.AddAsync(job)).Entity;
        await DataContext.SaveChangesAsync();

        return createdEntity;
    }

    public Task<IEnumerable<JobDal>> GetAllAsReadOnlyAsync()
    {
        return Task.FromResult<IEnumerable<JobDal>>(
            _jobs.OrderBy(c => c.CreationTime).AsNoTracking()
        );
    }

    public Task<IEnumerable<JobDal>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<JobDal>>(_jobs.OrderBy(c => c.CreationTime));
    }

    public Task<IEnumerable<JobDal>> GetJobsByFilterAsync(JobFilter jobFilter)
    {
        return Task.FromResult<IEnumerable<JobDal>>(
            _jobs.Where(jobFilter.GetPredicate()).OrderBy(c => c.CreationTime).AsQueryable()
        );
    }

    public async Task<JobDal> GetJobByIdAsync(Guid id)
    {
        return await _jobs.FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task<JobDal> GetJobByIdAsReadOnlyAsync(Guid id)
    {
        return await _jobs.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task<JobDal> UpdateJobAsync(JobDal job)
    {
        var updatingEntity = await _jobs.FirstOrDefaultAsync(j => j.Id == job.Id);
        if (updatingEntity == null)
            return null;

        _mapper.Map(job, updatingEntity);
        PrepareForUpdate(updatingEntity);

        await DataContext.SaveChangesAsync();

        return updatingEntity;
    }
}

internal class JobRepositoryMapperProfile : Profile
{
    public JobRepositoryMapperProfile()
    {
        CreateMap<JobDal, JobDal>();
    }
}
