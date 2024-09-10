using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Models.Preview;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Preview;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;

/// <summary>
/// Application service for performing operations with previews.
/// </summary>
public interface IPreviewAppService
{
    /// <summary>
    /// Returns a design info.
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <returns>The design info.</returns>
    /// <exception cref="NotFoundAppException">The line item was found.</exception>
    /// <exception cref="InvalidValueAppException"> The property of model has an invalid value.</exception>
    /// <exception cref="InvalidStateAppException"> The value of the model property conflicts with the current state of the application service.</exception>
    Task<DesignInfo> GetDesignInfoAsync(DesignInfoRequestAppModel model);

    /// <summary>
    /// Returns a proof file.
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <returns> The proof file.</returns>
    /// <exception cref="NotFoundAppException">The line item was found.</exception>
    /// <exception cref="InvalidValueAppException"> The property of model has an invalid value.</exception>
    /// <exception cref="InvalidStateAppException"> The value of the model property conflicts with the current state of the application service.</exception>
    Task<Stream> RenderProofForRecipientAsync(ProofRequestAppModel model);

    /// <summary>
    /// Returns a preview file.
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <returns> The preview file.</returns>
    /// <exception cref="NotFoundAppException">The line item was found.</exception>
    /// <exception cref="InvalidValueAppException"> The property of model has an invalid value.</exception>
    /// <exception cref="InvalidStateAppException"> The value of the model property conflicts with the current state of the application service.</exception>
    Task<Stream> RenderDesignPreviewAsync(PreviewRequestAppModel model);

    /// <summary>
    /// Returns a archive with proofs.
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <returns> The archive with proof files.</returns>
    /// <exception cref="NotFoundAppException">The line item was found.</exception>
    /// <exception cref="InvalidValueAppException"> The property of model has an invalid value.</exception>
    /// <exception cref="InvalidStateAppException"> The value of the model property conflicts with the current state of the application service.</exception>
    Task<ProofsZip> GeProofsZipAsync(ProofZipRequestAppModel model);
}
