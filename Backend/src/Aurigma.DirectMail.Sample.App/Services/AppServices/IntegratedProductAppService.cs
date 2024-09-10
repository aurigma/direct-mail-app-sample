using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Exceptions.IntegratedProduct;
using Aurigma.DirectMail.Sample.App.Exceptions.Product;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Models.IntegratedProduct;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.IntegratedProduct;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aurigma.DirectMail.Sample.App.Services.AppServices;

public class IntegratedProductAppService(
    IIntegratedProductService integratedProductService,
    IMapper mapper,
    ILogger<IntegratedProductAppService> logger
) : IIntegratedProductAppService
{
    private readonly IIntegratedProductService _integratedProductService = integratedProductService;
    private readonly ILogger<IntegratedProductAppService> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<List<IntegratedProduct>> GetIntegratedProductsAsync(
        IntegratedProductRequestModel model
    )
    {
        try
        {
            var integrationProducts = await _integratedProductService.GetIntegratedProductsAsync(
                _mapper.Map<IntegratedProductFilter>(model)
            );
            LogSuccessGetting(integrationProducts.Count, model);
            return integrationProducts;
        }
        catch (Exception ex)
        {
            LogErrorGetting(ex, model);
            throw;
        }
    }

    public async Task<List<IntegratedProductOption>> GetIntegratedProductOptionsAsync(Guid id)
    {
        try
        {
            var integratedProductOptions =
                await _integratedProductService.GetIntegratedProductOptionsAsync(id);
            LogSuccessGettingProductOptions(integratedProductOptions.Count, id);
            return integratedProductOptions;
        }
        catch (ProductNotFoundException ex)
        {
            LogGettingProductOptionsNotFound(ex, id);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (CustomersCanvasProductNotFoundException ex)
        {
            LogCustomersCanvasProductNotFound(ex, id.ToString());
            throw new InvalidStateAppException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorGettingProductOptions(ex, id);
            throw;
        }
    }

    public async Task<List<IntegratedProductTemplate>> GetIntegratedProductTemplatesAsync(
        Guid id,
        IntegrationProductOptionRequestModel model
    )
    {
        try
        {
            var integratedProductTemplates =
                await _integratedProductService.GetIntegratedProductTemplatesAsync(id, model);
            LogSuccessGettingProductTemplates(integratedProductTemplates.Count, id, model);
            return integratedProductTemplates;
        }
        catch (IntegratedProductOptionsException ex)
        {
            LogErrorAnOptionsInvalid(ex, model);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (ProductNotFoundException ex)
        {
            LogGettingProductOptionsNotFound(ex, id);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (CustomersCanvasDesignNotFoundException ex)
        {
            LogCustomersCanvasDesignNotFound(ex, id.ToString());
            throw new InvalidStateAppException("ProductReference", ex.Id, ex.Message, ex);
        }
        catch (CustomersCanvasProductNotFoundException ex)
        {
            LogCustomersCanvasProductNotFound(ex, id.ToString());
            throw new InvalidStateAppException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorGettingProductTemplates(ex, id, model);
            throw;
        }
    }

    public async Task<IntegratedProductTemplateDetails> GetIntegratedProductTemplateDetailsAsync(
        Guid id,
        IntegratedProductTemplateDetailsRequestModel model
    )
    {
        try
        {
            var templateDetails =
                await _integratedProductService.GetIntegratedProductTemplateDetailsAsync(id, model);
            LogSuccessGettingTemplatesDetails(id, model.TemplateId);
            return templateDetails;
        }
        catch (ProductNotFoundException ex)
        {
            LogGettingProductOptionsNotFound(ex, id);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (IntegratedProductOptionsException ex)
        {
            LogErrorAnOptionsInvalid(ex, model);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (CustomersCanvasDesignNotConnectedException ex)
        {
            LogCustomersCanvasDesignNotConnected(ex, id, model.TemplateId);
            throw new InvalidStateAppException(ex.Message, ex);
        }
        catch (CustomersCanvasDesignNotFoundException ex)
        {
            LogCustomersCanvasDesignNotFound(ex, id.ToString());
            throw new InvalidStateAppException(ex.Message, ex);
        }
        catch (CustomersCanvasProductNotFoundException ex)
        {
            LogCustomersCanvasProductNotFound(ex, id.ToString());
            throw new InvalidStateAppException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorGettingTemplatesDetails(ex, id, model.TemplateId);
            throw;
        }
    }

    public async Task UpdateIntegratedProductResourcesAsync(Guid productId)
    {
        try
        {
            await _integratedProductService.UpdateIntegratedProductResourcesAsync(productId);
            LogUpdateIntegratedProductResources(productId);
        }
        catch (ProductNotFoundException ex)
        {
            LogUpdateIntegratedProductResourcesNotFound(ex, productId);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (CustomersCanvasProductNotFoundException ex)
        {
            LogCustomersCanvasProductNotFound(ex, productId.ToString());
            throw new InvalidStateAppException("productReference", ex.Id, ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorUpdateIntegratedProductResources(ex, productId);
            throw;
        }
    }

    #region Logging

    private void LogSuccessGetting(int productsCount, IntegratedProductRequestModel model)
    {
        _logger.LogDebug(
            $" Integrated products was returned. products count={productsCount}, Request model={Serialize(model)}"
        );
    }

    private void LogErrorGetting(Exception ex, IntegratedProductRequestModel model)
    {
        _logger.LogError(
            ex,
            $"Failed to request a integrated products. Request model={Serialize(model)}"
        );
    }

    private void LogSuccessGettingProductOptions(int optionsCount, Guid id)
    {
        _logger.LogDebug(
            $"Integrated product options was returned. Entity id ={id}. optionsCount={optionsCount}"
        );
    }

    private void LogErrorGettingProductOptions(Exception ex, Guid id)
    {
        _logger.LogError(ex, $"Failed to request a integrated product options. productId={id}");
    }

    private void LogGettingProductOptionsNotFound(Exception ex, Guid id)
    {
        _logger.LogWarning(ex, $"The product was not found. productId={id}");
    }

    private void LogCustomersCanvasProductNotFound(Exception ex, string reference)
    {
        _logger.LogWarning(
            ex,
            $"The Customer's canvas product with reference {reference} was not found"
        );
    }

    private void LogCustomersCanvasDesignNotFound(Exception ex, string reference)
    {
        _logger.LogWarning(
            ex,
            $"The Customer's canvas design linked to the product with reference {reference} was not found"
        );
    }

    private void LogSuccessGettingProductTemplates(
        int templatesCount,
        Guid id,
        IntegrationProductOptionRequestModel options
    )
    {
        _logger.LogDebug(
            $"Integrated product templates was returned. ProductId ={id}. templatesCount={templatesCount}. options={Serialize(options)}"
        );
    }

    private void LogErrorGettingProductTemplates(
        Exception ex,
        Guid id,
        IntegrationProductOptionRequestModel options
    )
    {
        _logger.LogError(
            ex,
            $"Failed to request a integrated product templates. productId={id}. options={Serialize(options)}"
        );
    }

    private void LogErrorAnOptionsInvalid<T>(Exception ex, T options)
        where T : class
    {
        _logger.LogWarning(
            ex,
            $"Failed to request a product templates. options={Serialize(options)}"
        );
    }

    private void LogSuccessGettingTemplatesDetails(Guid id, string templateId)
    {
        _logger.LogDebug(
            $"Integrated product template details was returned. productId={id}. templateId={templateId}"
        );
    }

    private void LogErrorGettingTemplatesDetails(Exception ex, Guid id, string templateId)
    {
        _logger.LogError(
            ex,
            $"Failed to request a integrated product template details. productId={id}. templateId={templateId}"
        );
    }

    private void LogCustomersCanvasDesignNotConnected(Exception ex, Guid id, string templateId)
    {
        _logger.LogWarning(
            ex,
            $"The Customer's canvas design with id {templateId} was not linked to the product with reference {id}"
        );
    }

    private void LogUpdateIntegratedProductResources(Guid id)
    {
        _logger.LogDebug($"Integrated product resources was updated. productId={id}");
    }

    private void LogUpdateIntegratedProductResourcesNotFound(Exception ex, Guid productId)
    {
        _logger.LogWarning(ex, $"The product was not found. productId={productId}");
    }

    private void LogErrorUpdateIntegratedProductResources(Exception ex, Guid productId)
    {
        _logger.LogError(
            ex,
            $"Failed to update integrated product resources. productId={productId}"
        );
    }

    #endregion Logging

    private static string Serialize<T>(T source)
        where T : class
    {
        return StringHelper.Serialize(source);
    }
}

internal class IntegratedProductAppServiceMapperProfile : Profile
{
    public IntegratedProductAppServiceMapperProfile()
    {
        CreateMap<IntegratedProductRequestModel, IntegratedProductFilter>();
    }
}
