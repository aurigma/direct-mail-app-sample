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
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Repositories;

public class CampaignRepository : BaseRepository, ICampaignRepository
{
    private readonly DbSet<CampaignDal> _campaigns;
    private readonly IMapper _mapper;

    public CampaignRepository(DbContext context, IMapper mapper)
        : base(context)
    {
        _campaigns = DataContext.Set<CampaignDal>();
        _mapper = mapper;
    }

    public async Task<CampaignDal> CreateCampaignAsync(CampaignDal campaign)
    {
        PrepareForCreation(campaign);
        var createdEntity = (await _campaigns.AddAsync(campaign)).Entity;
        await DataContext.SaveChangesAsync();

        return createdEntity;
    }

    public async Task<CampaignDal> DeleteCampaignAsync(Guid id)
    {
        var deletingEntity = await _campaigns.FirstOrDefaultAsync(c => c.Id == id);
        if (deletingEntity == null)
            return null;

        _campaigns.Remove(deletingEntity);
        await DataContext.SaveChangesAsync();

        return deletingEntity;
    }

    public Task<IEnumerable<CampaignDal>> GetAllAsReadOnlyAsync()
    {
        return Task.FromResult<IEnumerable<CampaignDal>>(
            _campaigns
                .Include(c => c.LineItems)
                .Include(c => c.RecipientListCampaigns)
                .OrderBy(c => c.CreationTime)
                .AsNoTracking()
        );
    }

    public Task<IEnumerable<CampaignDal>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<CampaignDal>>(
            _campaigns
                .Include(c => c.LineItems)
                .Include(c => c.RecipientListCampaigns)
                .OrderBy(c => c.CreationTime)
        );
    }

    public Task<IEnumerable<CampaignDal>> GetCampaignByFilterAsync(CampaignFilter filter)
    {
        return Task.FromResult<IEnumerable<CampaignDal>>(
            _campaigns
                .Include(c => c.LineItems)
                .Include(c => c.RecipientListCampaigns)
                .Where(filter.GetPredicate())
                .OrderBy(c => c.CreationTime)
                .AsQueryable()
        );
    }

    public async Task<CampaignDal> GetCampaignByIdAsReadOnlyAsync(Guid id)
    {
        return await _campaigns
            .AsNoTracking()
            .Include(c => c.LineItems)
            .Include(c => c.RecipientListCampaigns)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<CampaignDal> GetCampaignByIdAsync(Guid id)
    {
        return await _campaigns
            .Include(c => c.RecipientListCampaigns)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<CampaignDal> UpdateCampaignAsync(CampaignDal campaign)
    {
        var updatingEntity = await _campaigns
            .Include(c => c.RecipientListCampaigns)
            .FirstOrDefaultAsync(c => c.Id == campaign.Id);

        if (updatingEntity == null)
            return null;

        updatingEntity.RecipientListCampaigns.Clear();

        _mapper.Map(campaign, updatingEntity);
        PrepareForUpdate(updatingEntity);

        await DataContext.SaveChangesAsync();

        return updatingEntity;
    }
}

internal class CampaignRepositoryMapperProfile : Profile
{
    public CampaignRepositoryMapperProfile()
    {
        CreateMap<CampaignDal, CampaignDal>();
    }
}
