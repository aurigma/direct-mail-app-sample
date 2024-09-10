using Aurigma.DirectMail.Sample.App.Configurations;
using Aurigma.DirectMail.Sample.App.Exceptions.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Models.Project;
using Aurigma.StorefrontApi;
using Microsoft.Extensions.Options;

namespace Aurigma.DirectMail.Sample.App.Adapters.CustomersCanvas;

public class ProjectAdapter(
    IProjectsApiClient projectsApiClient,
    IOptionsSnapshot<CustomersCanvasConfiguration> optionsSnapshot
) : IProjectAdapter
{
    private readonly IProjectsApiClient _projectsApiClient = projectsApiClient;
    private readonly CustomersCanvasConfiguration _configuration = optionsSnapshot.Value;

    public async Task<ProjectDto> CreateProjectAsync(
        ProjectCreationAdapterModel model,
        string token
    )
    {
        _projectsApiClient.AuthorizationToken = token;

        var project = await _projectsApiClient.CreateAsync(
            _configuration.StorefrontId,
            body: model.CreationProjectModel
        );

        return project;
    }

    public async Task<ProjectProcessingResultsDto> GetProcessingResultsAsync(
        int projectId,
        string token
    )
    {
        try
        {
            _projectsApiClient.AuthorizationToken = token;

            var results = await _projectsApiClient.GetProjectProcessingResultsAsync(projectId);
            return results;
        }
        catch (ApiClientException ex) when (ex.StatusCode == 404)
        {
            throw new CustomersCanvasProjectNotFoundException(
                projectId,
                $"The project with identifier {projectId} was not found."
            );
        }
    }

    public async Task RestartProcessingAsync(int projectId, string token)
    {
        try
        {
            _projectsApiClient.AuthorizationToken = token;

            await _projectsApiClient.RestartProjectProcessingAsync(projectId);
        }
        catch (ApiClientException ex) when (ex.StatusCode == 404)
        {
            throw new CustomersCanvasProjectNotFoundException(
                projectId,
                $"The project with identifier {projectId} was not found."
            );
        }
    }
}
