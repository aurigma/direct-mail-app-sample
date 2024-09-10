using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Exceptions.LineItem;
using Aurigma.DirectMail.Sample.App.Filters;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Models.LineItem;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.LineItem;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aurigma.DirectMail.Sample.App.Services.AppServices;

public class LineItemAppService(
    ILogger<LineItemAppService> logger,
    ILineItemService lineItemService,
    IMapper mapper
) : ILineItemAppService
{
    private readonly ILineItemService _lineItemService = lineItemService;
    private readonly ILogger<LineItemAppService> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<LineItem> CreateLineItemAsync(LineItemCreationModel model)
    {
        try
        {
            var lineItem = await _lineItemService.CreateLineItemAsync(_mapper.Map<LineItem>(model));
            LogSuccessCreation(lineItem);
            return lineItem;
        }
        catch (LineItemIdException ex)
        {
            LogAnCreateInvalid(ex, model);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (LineItemCampaignException ex)
        {
            LogAnCreateInvalid(ex, model);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (LineItemProductException ex)
        {
            LogAnCreateInvalid(ex, model);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorCreation(ex, model);
            throw;
        }
    }

    public async Task<LineItem> GetLineItemByIdAsync(Guid id)
    {
        try
        {
            var lineItem = await _lineItemService.GetLineItemAsync(id);
            LogSuccessGettingById(lineItem);
            return lineItem;
        }
        catch (LineItemNotFoundException ex)
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

    public async Task<IEnumerable<LineItem>> GetLineItemsAsync(LineItemRequestModel model)
    {
        try
        {
            var lineItems = await _lineItemService.GetLineItemsAsync(
                _mapper.Map<LineItemFilter>(model)
            );
            LogSuccessGetting(lineItems.Count, model);
            return lineItems;
        }
        catch (Exception ex)
        {
            LogErrorGetting(ex, model);
            throw;
        }
    }

    public async Task<LineItem> UpdateLineItemAsync(LineItemUpdateModel model)
    {
        try
        {
            var lineItem = await _lineItemService.UpdateLineItemAsync(_mapper.Map<LineItem>(model));
            LogAnUpdate(lineItem);
            return lineItem;
        }
        catch (LineItemIdException ex)
        {
            LogAnUpdateInvalid(ex, model);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (LineItemCampaignException ex)
        {
            LogAnUpdateInvalid(ex, model);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (LineItemProductException ex)
        {
            LogAnUpdateInvalid(ex, model);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (LineItemNotFoundException ex)
        {
            LogAnUpdateNotFound(ex, model.Id);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (CustomersCanvasProductVariantNotFoundException ex)
        {
            LogCustomersCanvasProductVariantNotFound(ex, ex.Id);
            throw new InvalidStateAppException(
                nameof(model.ProductVariantId),
                ex.Id,
                ex.Message,
                ex
            );
        }
        catch (CustomersCanvasDesignNotFoundException ex)
        {
            LogCustomersCanvasDesignNotFound(ex, model.TemplateId);
            throw new InvalidStateAppException(
                nameof(model.TemplateId),
                model.TemplateId,
                ex.Message,
                ex
            );
        }
        catch (CustomersCanvasPrivateDesignNotFoundException ex)
        {
            LogCustomersCanvasPrivateDesignNotFound(ex, model.DesignId);
            throw new InvalidStateAppException(
                nameof(model.DesignId),
                model.DesignId,
                ex.Message,
                ex
            );
        }
        catch (Exception ex)
        {
            LogAnUpdateError(ex, model);
            throw;
        }
    }

    public async Task FinishLineItemPersonalizationAsync(Guid id)
    {
        try
        {
            await _lineItemService.FinishLineItemPersonalizationAsync(id);
            LogSuccessFinishLineItemPersonalization(id);
        }
        catch (LineItemNotFoundException ex)
        {
            LogFinishLineItemPersonalizationNotFound(ex, id);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (LineItemCampaignException ex)
        {
            LogAnInvalidFinishLineItemPersonalization(ex, id);
            throw new InvalidStateAppException("Campaign", ex.Id.ToString(), ex.Message, ex);
        }
        catch (LineItemDesignException ex)
        {
            LogAnInvalidFinishLineItemPersonalization(ex, id);
            throw new InvalidStateAppException("Design", ex.Id?.ToString(), ex.Message, ex);
        }
        catch (CustomersCanvasPrivateDesignNotFoundException ex)
        {
            LogCustomersCanvasPrivateDesignNotFound(ex, ex.Id);
            throw new InvalidStateAppException("Design", ex.Id, ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorFinishLineItemPersonalization(ex, id);
            throw;
        }
    }

    #region Logging
    private void LogSuccessCreation(LineItem lineItem)
    {
        _logger.LogDebug(
            $"The line item was created. Id: {lineItem.Id}, lineItem={Serialize(lineItem)}"
        );
    }

    private void LogErrorCreation(Exception ex, LineItemCreationModel model)
    {
        _logger.LogError(ex, $"Failed to create a line item. Request model={Serialize(model)}");
    }

    private void LogAnCreateInvalid(Exception ex, LineItemCreationModel model)
    {
        _logger.LogWarning(ex, $"Failed to create the line item.  model={Serialize(model)}");
    }

    private void LogSuccessGetting(int lineItemsCount, LineItemRequestModel model)
    {
        _logger.LogDebug(
            $"Line items was returned. Line items count={lineItemsCount}, Request model={Serialize(model)}"
        );
    }

    private void LogErrorGetting(Exception ex, LineItemRequestModel model)
    {
        _logger.LogError(ex, $"Failed to request a line items. Request model={Serialize(model)}");
    }

    private void LogSuccessGettingById(LineItem model)
    {
        _logger.LogDebug(
            $"Line iten was returned by ID. Entity id ={model.Id}. lineItem={Serialize(model)}"
        );
    }

    private void LogErrorGettingById(Exception ex, Guid id)
    {
        _logger.LogError(ex, $"Failed to request a line item by ID. Entity id ={id}");
    }

    private void LogGettingNotFound(Exception ex, Guid id)
    {
        _logger.LogWarning(ex, $"The line item was not found. id={id}");
    }

    private void LogAnUpdate(LineItem lineItem)
    {
        _logger.LogDebug($"The line item was updated. lineItem={Serialize(lineItem)}");
    }

    private void LogAnUpdateNotFound(Exception ex, Guid id)
    {
        _logger.LogWarning(ex, $"The line item to update was not found. id={id}");
    }

    private void LogAnUpdateError(Exception ex, LineItemUpdateModel model)
    {
        _logger.LogError(ex, $"Failed to update the line item. model={Serialize(model)}");
    }

    private void LogAnUpdateInvalid(Exception ex, LineItemUpdateModel model)
    {
        _logger.LogWarning(ex, $"Failed to update the line item.  model={Serialize(model)}");
    }

    private void LogCustomersCanvasDesignNotFound(Exception ex, string id)
    {
        _logger.LogWarning(ex, $"The Customer's canvas design with id {id} was not found");
    }

    private void LogCustomersCanvasProductVariantNotFound(Exception ex, string variantId)
    {
        _logger.LogWarning(
            ex,
            $"The Customer's Canvas product variant with id: {variantId} was not found."
        );
    }

    private void LogCustomersCanvasPrivateDesignNotFound(Exception ex, string id)
    {
        _logger.LogWarning(ex, $"The Customer's canvas private design with id {id} was not found");
    }

    private void LogSuccessFinishLineItemPersonalization(Guid id)
    {
        _logger.LogDebug(
            $"The Line item personalization process has been successfully finished. id={id}"
        );
    }

    private void LogAnInvalidFinishLineItemPersonalization(Exception ex, Guid id)
    {
        _logger.LogWarning(
            ex,
            $"Failed to finished line item personalization process. lineItemId={id}"
        );
    }

    private void LogFinishLineItemPersonalizationNotFound(Exception ex, Guid id)
    {
        _logger.LogWarning(ex, $"The line item to finish personalization was not found. id={id}");
    }

    private void LogErrorFinishLineItemPersonalization(Exception ex, Guid id)
    {
        _logger.LogError(
            ex,
            $"Failed to finished line item personalization process. lineItemId={id}"
        );
    }

    #endregion Logging

    private static string Serialize<T>(T source)
        where T : class
    {
        return StringHelper.Serialize(source);
    }
}

internal class LineItemAppServiceMapperProfile : Profile
{
    public LineItemAppServiceMapperProfile()
    {
        CreateMap<LineItemRequestModel, LineItemFilter>();

        CreateMap<LineItemCreationModel, LineItem>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

        CreateMap<LineItemUpdateModel, LineItem>();
    }
}
