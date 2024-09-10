using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Exceptions.Product;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Models.LineItem;
using Aurigma.DirectMail.Sample.App.Models.Product;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.LineItem;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Product;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aurigma.DirectMail.Sample.App.Services.AppServices;

public class ProductAppService(
    ILogger<ProductAppService> logger,
    IProductService productService,
    IMapper mapper
) : IProductAppService
{
    private readonly IProductService _productService = productService;
    private readonly ILogger<ProductAppService> _logger = logger;
    private IMapper _mapper = mapper;

    public async Task<Product> CreateProductAsync(ProductCreationModel model)
    {
        try
        {
            var product = await _productService.CreateProductAsync(_mapper.Map<Product>(model));
            LogSuccessCreation(product);

            return product;
        }
        catch (Exception ex)
        {
            LogErrorCreation(ex, model);
            throw;
        }
    }

    public async Task<Product> GetProductByIdAsync(Guid id)
    {
        try
        {
            var product = await _productService.GetProductAsync(id);
            LogSuccessGettingById(product);

            return product;
        }
        catch (ProductNotFoundException ex)
        {
            LogGettingNotFound(ex, id);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorGettingById(ex, id);
            throw;
        }
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(ProductRequestModel model)
    {
        try
        {
            var products = await _productService.GetProductsAsync(
                _mapper.Map<ProductFilter>(model)
            );
            LogSuccessGetting(products.Count, model);
            return products;
        }
        catch (Exception ex)
        {
            LogErrorGetting(ex, model);
            throw;
        }
    }

    #region Logging
    private void LogSuccessCreation(Product product)
    {
        _logger.LogDebug(
            $"The product was created. Id: {product.Id}, product={Serialize(product)}"
        );
    }

    private void LogErrorCreation(Exception ex, ProductCreationModel model)
    {
        _logger.LogError(ex, $"Failed to create a product. Request model={Serialize(model)}");
    }

    private void LogSuccessGetting(int productsCount, ProductRequestModel model)
    {
        _logger.LogDebug(
            $" Products was returned. products count={productsCount}, Request model={Serialize(model)}"
        );
    }

    private void LogErrorGetting(Exception ex, ProductRequestModel model)
    {
        _logger.LogError(ex, $"Failed to request a products. Request model={Serialize(model)}");
    }

    private void LogSuccessGettingById(Product model)
    {
        _logger.LogDebug(
            $"Product was returned by ID. Entity id ={model.Id}. product={Serialize(model)}"
        );
    }

    private void LogErrorGettingById(Exception ex, Guid id)
    {
        _logger.LogError(ex, $"Failed to request a product by ID. Entity id ={id}");
    }

    private void LogGettingNotFound(Exception ex, Guid id)
    {
        _logger.LogError(ex, $"The product was not found. id={id}");
    }

    #endregion Logging

    private static string Serialize<T>(T source)
        where T : class
    {
        return StringHelper.Serialize(source);
    }
}

internal class ProductAppServiceMapperProfile : Profile
{
    public ProductAppServiceMapperProfile()
    {
        CreateMap<ProductCreationModel, Product>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
        CreateMap<ProductRequestModel, ProductFilter>();
    }
}
