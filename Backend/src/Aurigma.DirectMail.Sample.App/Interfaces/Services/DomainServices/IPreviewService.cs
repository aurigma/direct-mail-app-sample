using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Exceptions.Preview;
using Aurigma.DirectMail.Sample.App.Models.Preview;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Preview;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;

/// <summary>
/// Domain service for performing operations with previews.
/// </summary>
public interface IPreviewService
{
    /// <summary>
    /// Returns a design info.
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <returns>The design info.</returns>
    /// <exception cref="PreviewIdException">The IDs has an invalid value.</exception>
    /// <exception cref="CustomersCanvasPrivateDesignNotFoundException">The private design was not found.</exception>
    /// <exception cref="PreviewLineItemException">The line item has an invalid value.</exception>
    Task<DesignInfo> GetDesignInfoAsync(DesignInfoRequestAppModel model);

    /// <summary>
    /// Returns a proof file.
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <returns>The proof file.</returns>
    /// <exception cref="PreviewRecipientException">The preview's recipient has an invalid value.</exception>
    /// <exception cref="PreviewIdException">The IDs has an invalid value.</exception>
    /// <exception cref="CustomersCanvasPrivateDesignNotFoundException">The private design was not found.</exception>
    /// <exception cref="PreviewLineItemException">The line item has an invalid value.</exception>
    Task<Stream> RenderProofForRecipientAsync(ProofRequestAppModel model);

    /// <summary>
    /// Returns a preview file.
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <returns>The preview file.</returns>
    /// <exception cref="PreviewIdException">The IDs has an invalid value.</exception>
    /// <exception cref="CustomersCanvasPrivateDesignNotFoundException">The private design was not found.</exception>
    /// <exception cref="CustomersCanvasDesignNotConnectedException">The private design was not found.</exception>
    /// <exception cref="PreviewLineItemException">The line item has an invalid value.</exception>
    Task<Stream> RenderDesignPreviewAsync(PreviewRequestAppModel model);

    /// <summary>
    /// Returns a archive with proofs.
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <param name="recipientsCount">How many recipients are included in the archive.</param>
    /// <returns>The archive with proof files.</returns>
    /// <exception cref="PreviewRecipientException">The preview's recipient has an invalid value.</exception>
    /// <exception cref="CustomersCanvasPrivateDesignNotFoundException">Private design was not found.</exception>
    /// <exception cref="PreviewLineItemException">The line item has an invalid value.</exception>
    Task<ProofsZip> GetProofsZipAsync(ProofZipRequestAppModel model, int recipientsCount = 3);
}
