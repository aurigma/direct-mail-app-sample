using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Exceptions.LineItem;
using Aurigma.DirectMail.Sample.App.Exceptions.Preview;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Models.Preview;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Preview;
using Microsoft.Extensions.Logging;

namespace Aurigma.DirectMail.Sample.App.Services.AppServices;

public class PreviewAppService(IPreviewService previewService, ILogger<PreviewAppService> logger)
    : IPreviewAppService
{
    private readonly IPreviewService _previewService = previewService;
    private readonly ILogger<PreviewAppService> _logger = logger;

    public async Task<DesignInfo> GetDesignInfoAsync(DesignInfoRequestAppModel model)
    {
        try
        {
            var designInfo = await _previewService.GetDesignInfoAsync(model);
            LogSuccessRequestDesignInfo(designInfo);
            return designInfo;
        }
        catch (PreviewIdException ex)
        {
            LogAnInvalidRequestDesignInfo(ex, model);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (LineItemNotFoundException ex)
        {
            LogAnInvalidRequestDesignInfo(ex, model);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (PreviewLineItemException ex)
        {
            LogAnInvalidRequestDesignInfo(ex, model);
            throw new InvalidStateAppException("LineItem", ex.Id.ToString(), ex.Message, ex);
        }
        catch (CustomersCanvasPrivateDesignNotFoundException ex)
        {
            LogCustomersCanvasDesignNotFound(ex, ex.Id);
            throw new InvalidStateAppException("DesignId", ex.Id, ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorRequestDesignInfo(ex, model);
            throw;
        }
    }

    public async Task<Stream> RenderProofForRecipientAsync(ProofRequestAppModel model)
    {
        try
        {
            var renderedFile = await _previewService.RenderProofForRecipientAsync(model);
            LogSuccessRequestRenderDesignProof(model);
            return renderedFile;
        }
        catch (PreviewRecipientException ex)
        {
            LogAnInvalidRequestRenderDesignProof(ex, model);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (PreviewIdException ex)
        {
            LogAnInvalidRequestRenderDesignProof(ex, model);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (LineItemNotFoundException ex)
        {
            LogAnInvalidRequestRenderDesignProof(ex, model);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (PreviewLineItemException ex)
        {
            LogAnInvalidRequestRenderDesignProof(ex, model);
            throw new InvalidStateAppException("LineItem", ex.Id.ToString(), ex.Message, ex);
        }
        catch (CustomersCanvasPrivateDesignNotFoundException ex)
        {
            LogCustomersCanvasDesignNotFound(ex, ex.Id);
            throw new InvalidStateAppException("DesignId", ex.Id, ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorRequestRenderDesignProof(ex, model);
            throw;
        }
    }

    public async Task<Stream> RenderDesignPreviewAsync(PreviewRequestAppModel model)
    {
        try
        {
            var renderedPreview = await _previewService.RenderDesignPreviewAsync(model);
            LogSuccessRequestRenderDesignPreview(model);
            return renderedPreview;
        }
        catch (PreviewRecipientException ex)
        {
            LogAnInvalidRequestRenderDesignPreview(ex, model);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (PreviewIdException ex)
        {
            LogAnInvalidRequestRenderDesignPreview(ex, model);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (LineItemNotFoundException ex)
        {
            LogAnInvalidRequestRenderDesignPreview(ex, model);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (PreviewLineItemException ex)
        {
            LogAnInvalidRequestRenderDesignPreview(ex, model);
            throw new InvalidStateAppException("LineItem", ex.Id.ToString(), ex.Message, ex);
        }
        catch (CustomersCanvasPrivateDesignNotFoundException ex)
        {
            LogCustomersCanvasDesignNotFound(ex, ex.Id);
            throw new InvalidStateAppException("DesignId", ex.Id, ex.Message, ex);
        }
        catch (CustomersCanvasDesignNotConnectedException ex)
        {
            LogCustomersCanvasDesignNotFound(ex, ex.DesignId);
            throw new InvalidStateAppException("DesignId", ex.DesignId, ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorRequestRenderDesignPreview(ex, model);
            throw;
        }
    }

    public async Task<ProofsZip> GeProofsZipAsync(ProofZipRequestAppModel model)
    {
        try
        {
            var zip = await _previewService.GetProofsZipAsync(model);
            LogSuccessRequestProofsZip(model);
            return zip;
        }
        catch (PreviewIdException ex)
        {
            LogAnInvalidRequestProofsZip(ex, model);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (PreviewRecipientException ex)
        {
            LogAnInvalidRequestProofsZip(ex, model);
            throw new InvalidValueAppException(ex.Message, ex);
        }
        catch (LineItemNotFoundException ex)
        {
            LogAnInvalidRequestProofsZip(ex, model);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (PreviewLineItemException ex)
        {
            LogAnInvalidRequestProofsZip(ex, model);
            throw new InvalidStateAppException("LineItem", ex.Id.ToString(), ex.Message, ex);
        }
        catch (CustomersCanvasPrivateDesignNotFoundException ex)
        {
            LogCustomersCanvasDesignNotFound(ex, ex.Id);
            throw new InvalidStateAppException("DesignId", ex.Id, ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorRequestProofsZip(ex, model);
            throw;
        }
    }

    private void LogSuccessRequestDesignInfo(DesignInfo designInfo)
    {
        _logger.LogDebug($"The design info was returned. info={Serialize(designInfo)}");
    }

    private void LogAnInvalidRequestDesignInfo(Exception ex, DesignInfoRequestAppModel model)
    {
        _logger.LogWarning(ex, $"Failed to request a design info. requestModel={model}");
    }

    private void LogErrorRequestDesignInfo(Exception ex, DesignInfoRequestAppModel model)
    {
        _logger.LogError(ex, $"Failed to request a design info. requestModel={model}");
    }

    private void LogSuccessRequestRenderDesignProof(ProofRequestAppModel model)
    {
        _logger.LogDebug($"The design proof was rendered. requestModel={Serialize(model)}");
    }

    private void LogAnInvalidRequestRenderDesignProof(Exception ex, ProofRequestAppModel model)
    {
        _logger.LogWarning(
            ex,
            $"Failed to request a render design proof. requestModel={Serialize(model)}"
        );
    }

    private void LogErrorRequestRenderDesignProof(Exception ex, ProofRequestAppModel model)
    {
        _logger.LogError(
            ex,
            $"Failed to request a render design proof. requestModel={Serialize(model)}"
        );
    }

    private void LogSuccessRequestRenderDesignPreview(PreviewRequestAppModel model)
    {
        _logger.LogDebug($"The design preview was rendered. requestModel={Serialize(model)}");
    }

    private void LogAnInvalidRequestRenderDesignPreview(Exception ex, PreviewRequestAppModel model)
    {
        _logger.LogWarning(
            ex,
            $"Failed to request a render design preview. requestModel={Serialize(model)}"
        );
    }

    private void LogErrorRequestRenderDesignPreview(Exception ex, PreviewRequestAppModel model)
    {
        _logger.LogError(
            ex,
            $"Failed to request a render design preview. requestModel={Serialize(model)}"
        );
    }

    private void LogSuccessRequestProofsZip(ProofZipRequestAppModel model)
    {
        _logger.LogDebug($"The proofs zip archive was created. requestModel={Serialize(model)}");
    }

    private void LogCustomersCanvasDesignNotFound(Exception ex, string id)
    {
        _logger.LogWarning(
            ex,
            $"The private design was not found from Asset Storage. privateDesignId={id}"
        );
    }

    private void LogCustomersCanvasDesignNotConnected(Exception ex, string id)
    {
        _logger.LogWarning(
            ex,
            $"The design was not connected to product variant. privateDesignId={id}"
        );
    }

    private void LogAnInvalidRequestProofsZip(Exception ex, ProofZipRequestAppModel model)
    {
        _logger.LogWarning(
            ex,
            $"Failed to request a proofs zip archive. requestModel={Serialize(model)}"
        );
    }

    private void LogErrorRequestProofsZip(Exception ex, ProofZipRequestAppModel model)
    {
        _logger.LogError(
            ex,
            $"Failed to request a proofs zip archive. requestModel={Serialize(model)}"
        );
    }

    private static string Serialize<T>(T source)
        where T : class
    {
        return StringHelper.Serialize(source);
    }
}
