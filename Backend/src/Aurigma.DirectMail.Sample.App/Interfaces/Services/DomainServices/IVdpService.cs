using Aurigma.DesignAtoms.Model;
using Aurigma.DirectMail.Sample.App.Models.VDP;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.RecipientList;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Variable;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;

/// <summary>
/// Domain service for performing operations with VDP data.
/// </summary>
public interface IVdpService
{
    /// <summary>
    /// Sava a VDP data to design.
    /// </summary>
    /// <param name="model">Request model.</param>
    Task SendVdpDataAsync(VdpSendDataAppModel model);

    /// <summary>
    /// Returns a variables for recipient.
    /// </summary>
    /// <param name="recipient">Recipient.</param>
    /// <returns>Collection of variables.</returns>
    IEnumerable<Variable> GetRecipientVariableData(
        Recipient recipient,
        List<VdpBuildDataSetImageAppModel> images
    );

    /// <summary>
    /// Builds a VDP data set according to the obtained variables
    /// </summary>
    /// <param name="surfaceIndexes">Array of surface indexes.</param>
    /// <param name="variables">VDP variables.</param>
    /// <returns>VDP dataset.</returns>
    DataSet BuildVdpDataSet(int[] surfaceIndexes, Dictionary<Guid, List<Variable>> variables);

    /// <summary>
    /// Returns a missing variable names from variable collection.
    /// </summary>
    /// <param name="variables">Collection of variables.</param>
    /// <returns>Missing variable names</returns>
    VariableValidationResult ValidateVariables(List<Variable> variables);

    /// <summary>
    /// Returns a available variables.
    /// </summary>
    /// <param name="images">Linked images</param>
    /// <returns>The collection of variables.</returns>
    IEnumerable<VariableInfo> GetAvailableVariables();
}
