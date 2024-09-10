using Aurigma.DirectMail.Sample.App.Models.Project;
using Aurigma.StorefrontApi;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;

/// <summary>
/// Adapter for performing operations with Customer's Canvas projects.
/// </summary>
public interface IProjectAdapter
{
    /// <summary>
    /// Creates a project in Customer's Canvas
    /// </summary>
    /// <param name="model">Request model.</param>
    /// <param name="token">Customer's canvas authorization token.</param>
    /// <returns>Created project DTO.</returns>
    Task<ProjectDto> CreateProjectAsync(ProjectCreationAdapterModel model, string token);

    /// <summary>
    /// Returns a project processing results.
    /// </summary>
    /// <param name="projectId">Customer's Canvas project id.</param>
    /// <param name="token">Customer's canvas authorization token.</param>
    /// <returns>The project processing results.</returns>
    Task<ProjectProcessingResultsDto> GetProcessingResultsAsync(int projectId, string token);

    /// <summary>
    /// Restarts job processing.
    /// </summary>
    /// <param name="projectId">Customer's Canvas project id.</param>
    /// <param name="token">Customer's canvas authorization token.</param>
    Task RestartProcessingAsync(int projectId, string token);
}
