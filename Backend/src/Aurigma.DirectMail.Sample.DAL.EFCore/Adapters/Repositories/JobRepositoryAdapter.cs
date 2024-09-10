using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;
using Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Job;
using AutoMapper;
using DomainStorage = Aurigma.DirectMail.Sample.App.Interfaces.Storages;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Adapters.Repositories;

public class JobRepositoryAdapter(IJobRepository jobRepository, IMapper mapper)
    : DomainStorage.IJobRepository
{
    private readonly IJobRepository _jobRepository = jobRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Job> CreateJobAsync(Job job)
    {
        return _mapper.Map<Job>(await _jobRepository.CreateJobAsync(_mapper.Map<JobDal>(job)));
    }

    public async Task<List<Job>> GetAllAsReadOnlyAsync()
    {
        return _mapper.Map<List<Job>>(await _jobRepository.GetAllAsReadOnlyAsync());
    }

    public async Task<List<Job>> GetJobsByFilterAsync(JobFilter jobFilter)
    {
        return _mapper.Map<List<Job>>(await _jobRepository.GetJobsByFilterAsync(jobFilter));
    }

    public async Task<Job> GetJobByIdAsReadOnlyAsync(Guid id)
    {
        return _mapper.Map<Job>(await _jobRepository.GetJobByIdAsReadOnlyAsync(id));
    }

    public async Task<Job> GetJobByIdAsync(Guid id)
    {
        return _mapper.Map<Job>(await _jobRepository.GetJobByIdAsync(id));
    }

    public async Task<Job> UpdateJobAsync(Job job)
    {
        return _mapper.Map<Job>(await _jobRepository.UpdateJobAsync(_mapper.Map<JobDal>(job)));
    }
}

internal class JobRepositoryAdapterMapperProfile : Profile
{
    public JobRepositoryAdapterMapperProfile()
    {
        CreateMap<Job, JobDal>();
        CreateMap<JobDal, Job>();
    }
}
