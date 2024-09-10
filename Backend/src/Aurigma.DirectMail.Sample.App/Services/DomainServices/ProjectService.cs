using Aurigma.DesignAtoms.ExtensionMethods;
using Aurigma.DirectMail.Sample.App.Configurations;
using Aurigma.DirectMail.Sample.App.Interfaces.Adapters.CustomersCanvas;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Models.Project;
using Aurigma.StorefrontApi;
using Microsoft.Extensions.Options;

namespace Aurigma.DirectMail.Sample.App.Services.DomainServices;

public class ProjectService(
    IProjectAdapter projectAdapter,
    ITokenAdapter tokenAdapter,
    IOptionsSnapshot<CustomersCanvasConfiguration> optionsSnapshot
) : IProjectService
{
    private readonly IProjectAdapter _projectAdapter = projectAdapter;
    private readonly ITokenAdapter _tokenAdapter = tokenAdapter;
    private readonly CustomersCanvasConfiguration _configuration = optionsSnapshot.Value;

    public async Task<ProjectDto> CreateProjectAsync(ProjectCreationAppModel model)
    {
        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        var creationProjectAdapterModel = new ProjectCreationAdapterModel()
        {
            CreationProjectModel = new CreateProjectDto()
            {
                CustomerId = model.Campaign.Id.ToString(),
                CustomerName = model.Company.Name,
                OrderId = model.Campaign.Id.ToString(),
                OrderLineItemId = model.LineItem.Id.ToString(),
                Name = model.Campaign.Title,
                ProductReference = model.LineItem.ProductId.ToString(),
                OwnerId = model.Campaign.Id.ToString(),
                Items = new List<ProjectItemParametersDto>()
                {
                    new ProjectItemParametersDto
                    {
                        DesignIds = new List<string>() { model.DesignId },
                        Quantity = model.LineItem.Quantity,
                        Sku = model.LineItem.ProductId.ToString(),
                        Fields = model.Fields,
                    },
                },
            },
        };

        var projectDto = await _projectAdapter.CreateProjectAsync(
            creationProjectAdapterModel,
            token
        );

        return projectDto;
    }

    public async Task<ProjectProcessingResultsDto> GetProcessingResultsAsync(
        ProcessingResultsRequestAppModel model
    )
    {
        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        var processingResults = await _projectAdapter.GetProcessingResultsAsync(
            model.ProjectId,
            token
        );

        if (processingResults.OutputFileDetails.Any())
            ReplaceProcessingResultsUrls(processingResults);

        return processingResults;
    }

    public async Task RestartProcessingAsync(int projectId)
    {
        var token = await _tokenAdapter.GetCustomersCanvasTokenAsync();
        await _projectAdapter.RestartProcessingAsync(projectId, token);
    }

    private ProjectProcessingResultsDto ReplaceProcessingResultsUrls(
        ProjectProcessingResultsDto results
    )
    {
        foreach (var item in results.OutputFileDetails)
        {
            var builder = new UriBuilder(item.Url);
            builder.Host = new Uri(_configuration.ApiGatewayUrl).Host;
            builder.Scheme = "https";
            item.Url = builder.Uri.ToString();
        }

        return results;
    }
}
