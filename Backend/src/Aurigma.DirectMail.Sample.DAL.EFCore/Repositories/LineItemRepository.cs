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

public class LineItemRepository : BaseRepository, ILineItemRepository
{
    private readonly DbSet<LineItemDal> _lineItems;
    private readonly IMapper _mapper;

    public LineItemRepository(DbContext context, IMapper mapper)
        : base(context)
    {
        _lineItems = DataContext.Set<LineItemDal>();
        _mapper = mapper;
    }

    public async Task<LineItemDal> CreateLineItemAsync(LineItemDal lineItem)
    {
        PrepareForCreation(lineItem);

        var createdEntity = (await _lineItems.AddAsync(lineItem)).Entity;
        await DataContext.SaveChangesAsync();

        return createdEntity;
    }

    public async Task<LineItemDal> DeleteLineItemAsync(Guid id)
    {
        var deletingEntity = await _lineItems.FirstOrDefaultAsync(x => x.Id == id);
        if (deletingEntity == null)
            return null;

        _lineItems.Remove(deletingEntity);
        await DataContext.SaveChangesAsync();

        return deletingEntity;
    }

    public Task<IEnumerable<LineItemDal>> GetAllAsReadOnlyAsync()
    {
        return Task.FromResult<IEnumerable<LineItemDal>>(
            _lineItems.Include(l => l.Product).Include(l => l.Campaign).AsNoTracking()
        );
    }

    public Task<IEnumerable<LineItemDal>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<LineItemDal>>(
            _lineItems.Include(l => l.Product).Include(l => l.Campaign).Include(l => l.Jobs)
        );
    }

    public async Task<LineItemDal> GetLineItemByIdAsReadOnlyAsync(Guid id)
    {
        return await _lineItems
            .AsNoTracking()
            .Include(l => l.Product)
            .Include(l => l.Campaign)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<LineItemDal> GetLineItemByIdAsync(Guid id)
    {
        return await _lineItems
            .Include(l => l.Product)
            .Include(l => l.Campaign)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public Task<IEnumerable<LineItemDal>> GetLineItemsByFilterAsync(LineItemFilter filter)
    {
        return Task.FromResult<IEnumerable<LineItemDal>>(
            _lineItems
                .Include(l => l.Product)
                .Include(l => l.Campaign)
                .Where(filter.GetPredicate())
                .AsQueryable()
        );
    }

    public async Task<LineItemDal> UpdateLineItemAsync(LineItemDal lineItem)
    {
        var updatingEntity = await _lineItems.FirstOrDefaultAsync(l => l.Id == lineItem.Id);

        if (updatingEntity == null)
            return null;

        _mapper.Map(lineItem, updatingEntity);
        PrepareForUpdate(updatingEntity);

        await DataContext.SaveChangesAsync();

        return updatingEntity;
    }
}

internal class LineItemRepositoryMapperProfile : Profile
{
    public LineItemRepositoryMapperProfile()
    {
        CreateMap<LineItemDal, LineItemDal>();
    }
}
