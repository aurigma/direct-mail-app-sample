using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.DAL.EFCore.Entities;

namespace Aurigma.DirectMail.Sample.DAL.EFCore.Interfaces.Repositories;

public interface IProductRepository
{
    Task<ProductDal> GetProductByIdAsync(Guid id);
    Task<ProductDal> GetProductByIdAsReadOnlyAsync(Guid id);

    Task<IEnumerable<ProductDal>> GetAllAsync();
    Task<IEnumerable<ProductDal>> GetAllAsReadOnlyAsync();
    Task<IEnumerable<ProductDal>> GetProductsByFilterAsync(ProductFilter filter);

    Task<ProductDal> CreateProductAsync(ProductDal product);
    Task<ProductDal> UpdateProductAsync(ProductDal product);
    Task<ProductDal> DeleteProductAsync(Guid id);
}
