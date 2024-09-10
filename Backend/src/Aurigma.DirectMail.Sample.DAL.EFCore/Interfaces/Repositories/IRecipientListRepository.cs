using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;

public interface IRecipientListRepository
{
    Task<RecipientListDal> CreateRecipientListAsync(RecipientListDal list);
    Task<IEnumerable<RecipientListDal>> GetAllAsReadOnlyAsync();
    Task<RecipientListDal> GeRecipientListAsReadOnlyAsync(Guid id);

    Task<IEnumerable<RecipientListDal>> GetRecipientListsByFilterAsync(RecipientListFilter filter);
}
