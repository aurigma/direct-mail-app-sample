import * as API from "@/core/direct-mail-api-client";

export const apiClient = () => {
  const config = useRuntimeConfig();

  const apiConfig = {
    apiUrl: config.public.apiBase,
  } as API.ApiClientConfiguration;

  return {
    BuildInfo: new API.BuildInfoApiClient(apiConfig) as API.IBuildInfoApiClient,
    Company: new API.CompanyApiClient(apiConfig) as API.ICompanyApiClient,
    Campaign: new API.CampaignApiClient(apiConfig) as API.ICampaignApiClient,
    Category: new API.CategoryApiClient(apiConfig) as API.ICategoryApiClient,
    Product: new API.ProductApiClient(apiConfig) as API.IProductApiClient,
    LineItems: new API.LineItemApiClient(apiConfig) as API.ILineItemApiClient,
    IntegratedProduct: new API.IntegratedProductApiClient(
      apiConfig,
    ) as API.IIntegratedProductApiClient,
    RecipientList: new API.RecipientListApiClient(
      apiConfig,
    ) as API.RecipientListApiClient,
    EditorInfo: new API.EditorApiClient(apiConfig) as API.EditorApiClient,
    Preview: new API.PreviewApiClient(apiConfig) as API.PreviewApiClient,
    Job: new API.JobApiClient(apiConfig) as API.JobApiClient,
    ApiClient: new API.ApiClient(apiConfig) as API.ApiClient,
  };
};

export const baseUrlEditor = () => {
  const config = useRuntimeConfig();
  return config.public.urlEditor;
};
