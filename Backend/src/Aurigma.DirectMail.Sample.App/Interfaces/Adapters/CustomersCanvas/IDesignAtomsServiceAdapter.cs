using Aurigma.DesignAtomsApi;
using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Models.Preview;
using Aurigma.DirectMail.Sample.App.Models.VDP;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;

/// <summary>
/// Adapter for performing operations with Customer's Canvas Design Atoms service.
/// </summary>
public interface IDesignAtomsServiceAdapter
{
    /// <summary>
    /// Returns a design proof file.
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <returns>Design proof file.</returns>
    /// <remarks>Design proof - preview as close as possible to the final file.</remarks>
    /// <exception cref="CustomersCanvasPrivateDesignNotFoundException">The private design was not found.</exception>
    Task<FileResponse> RenderDesignProofAsync(ProofRequestAdapterModel model, string token);

    /// <summary>
    /// Returns a design preview file.
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <returns>Design preview file.</returns>
    /// <exception cref="CustomersCanvasPrivateDesignNotFoundException">The private design was not found.</exception>
    Task<FileResponse> RenderDesignPreviewAsync(PreviewRequestAdapterModel model, string token);

    /// <summary>
    /// Returns a design file variables.
    /// </summary>
    /// <param name="designId">Private design id.</param>
    /// <param name="userId"> Storefront user id.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <returns> Collection of variables.</returns>
    /// <exception cref="CustomersCanvasPrivateDesignNotFoundException">The private design was not found.</exception>
    Task<VariablesModel> GetVariablesAsync(string designId, string userId, string token);

    /// <summary>
    /// Saves a VDP to design.
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    Task SendVdpDataAsync(VdpSendDataAdapterModel model, string token);

    /// <summary>
    /// Returns a generated preview resource.
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <param name="token">Customer's Canvas authorization token.</param>
    /// <returns>Generated resource.</returns>
    Task<ResourceInfoDto> RenderDesignPreviewToResourceAsync(
        DesignPreviewToResourceRequestAdapterModel model,
        string token
    );
}
