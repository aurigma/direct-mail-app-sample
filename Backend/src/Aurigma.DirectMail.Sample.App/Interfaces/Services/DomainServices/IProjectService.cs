using Aurigma.DirectMail.Sample.App.Models.Project;
using Aurigma.StorefrontApi;

namespace Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;

public interface IProjectService
{
    Task<ProjectDto> CreateProjectAsync(ProjectCreationAppModel model);
    Task<ProjectProcessingResultsDto> GetProcessingResultsAsync(
        ProcessingResultsRequestAppModel model
    );

    Task RestartProcessingAsync(int projectId);
}
