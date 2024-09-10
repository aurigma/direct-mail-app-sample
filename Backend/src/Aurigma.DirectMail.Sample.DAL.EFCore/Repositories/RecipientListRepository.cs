using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;
using Aurigma.DirectMail.Sample.DAL.EFCore.Extensions;
using Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Repositories;

public class RecipientListRepository : BaseRepository, IRecipientListRepository
{
    private readonly DbSet<RecipientListDal> _lists;

    public RecipientListRepository(DbContext context)
        : base(context)
    {
        _lists = DataContext.Set<RecipientListDal>();
    }

    public async Task<RecipientListDal> CreateRecipientListAsync(RecipientListDal list)
    {
        PrepareForCreation(list);
        var createdEntity = (await _lists.AddAsync(list)).Entity;
        await DataContext.SaveChangesAsync();

        return createdEntity;
    }

    public Task<IEnumerable<RecipientListDal>> GetAllAsReadOnlyAsync()
    {
        return Task.FromResult<IEnumerable<RecipientListDal>>(
            _lists
                .Include(l => l.Recipients)
                .ThenInclude(r => r.RecipientImageRecipients)
                .ThenInclude(ri => ri.RecipientImage)
                .Include(l => l.RecipientListCampaigns)
                .OrderBy(l => l.CreationTime)
                .AsNoTracking()
        );
    }

    public Task<IEnumerable<RecipientListDal>> GetRecipientListsByFilterAsync(
        RecipientListFilter filter
    )
    {
        return Task.FromResult<IEnumerable<RecipientListDal>>(
            _lists
                .Include(l => l.Recipients)
                .ThenInclude(r => r.RecipientImageRecipients)
                .ThenInclude(ri => ri.RecipientImage)
                .Include(l => l.RecipientListCampaigns)
                .Where(filter.GetPredicate())
                .AsQueryable()
        );
    }

    public async Task<RecipientListDal> GeRecipientListAsReadOnlyAsync(Guid id)
    {
        return await _lists
            .AsNoTracking()
            .Include(l => l.Recipients)
            .ThenInclude(r => r.RecipientImageRecipients)
            .ThenInclude(ri => ri.RecipientImage)
            .Include(l => l.RecipientListCampaigns)
            .FirstOrDefaultAsync(l => l.Id == id);
    }
}
