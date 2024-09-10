using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;

public interface ILineItemRepository
{
    Task<LineItemDal> GetLineItemByIdAsync(Guid id);
    Task<LineItemDal> GetLineItemByIdAsReadOnlyAsync(Guid id);

    Task<IEnumerable<LineItemDal>> GetAllAsync();
    Task<IEnumerable<LineItemDal>> GetAllAsReadOnlyAsync();
    Task<IEnumerable<LineItemDal>> GetLineItemsByFilterAsync(LineItemFilter filter);

    Task<LineItemDal> CreateLineItemAsync(LineItemDal lineItem);
    Task<LineItemDal> UpdateLineItemAsync(LineItemDal lineItem);
    Task<LineItemDal> DeleteLineItemAsync(Guid id);
}
