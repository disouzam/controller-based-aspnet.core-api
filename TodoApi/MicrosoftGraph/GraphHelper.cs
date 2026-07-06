using System;
using System.Linq;
using System.Threading.Tasks;

using Azure.Core;
using Azure.Identity;

using Microsoft.Graph;

using TodoApi.Dtos;
using TodoApi.MicrosoftEntraId;

namespace TodoApi.MicrosoftGraph;

class GraphHelper
{
    // Settings object
    private static Settings? settings;

    // App-only auth token credential
    private static ClientSecretCredential? clientSecretCredential;

    // Client configured with app-only authentication
    private static GraphServiceClient? appClient;

    public static void InitializeGraphForAppOnlyAuth(Settings settings)
    {
        GraphHelper.settings = settings;

        if (settings == null)
        {
            throw new NullReferenceException("Settings cannot be null");
        }

        var tenantIdStr = GraphHelper.settings.TenantId.ToString();
        var clientIdStr = GraphHelper.settings.ClientId.ToString();
        var clientSecretStr = GraphHelper.settings.ClientSecret;

        clientSecretCredential ??= new ClientSecretCredential(tenantIdStr, clientIdStr, clientSecretStr);
        // Use the default scope, which will request the scopes configured on the app registration
        var scopes = new[] { ".default" };

        appClient ??= new GraphServiceClient(clientSecretCredential, scopes);
    }

    public static async Task<string> GetAppOnlyTokenAsync()
    {
        // Ensure credential isn't null
        _ = clientSecretCredential ??
            throw new NullReferenceException("Graph has not been initialized for app-only auth");

        // Request token with given scopes
        var context = new TokenRequestContext(["https://graph.microsoft.com/.default"]);
        var response = await clientSecretCredential.GetTokenAsync(context);
        var token = response.Token;
        return token;
    }

    public static async Task<SecretExpirationDateDto> GetSecretExpirationDateAsync(Guid secretId)
    {
        // Ensure app client isn't null
        _ = appClient ??
            throw new NullReferenceException("Graph has not been initialized for app-only auth");

        var appId = settings?.ClientId.ToString() ?? throw new NullReferenceException("Settings cannot be null");
        var accessToken = await GetAppOnlyTokenAsync();

        var application = await appClient.Applications
        .GetAsync(requestConfig =>
        {
            requestConfig.QueryParameters.Filter = $"appId eq '{appId}'";
            requestConfig.QueryParameters.Select = new[] { "id", "displayName", "passwordCredentials" };
        });

        var appDetails = application?.Value?.FirstOrDefault();

        if (appDetails is null)
        {
            throw new Exception($"Application with Client ID {appId} not found");
        }

        var secretDetails = appDetails.PasswordCredentials.FirstOrDefault(pc => pc.KeyId == secretId);

        if (secretDetails is null)
        {
            throw new Exception($"Secret with ID {secretId} not found");
        }

        // Example implementation, replace with actual Graph API call
        var result = new SecretExpirationDateDto
        {
            SecretId = secretId,
            StartDate = secretDetails.StartDateTime?.DateTime,
            ExpirationDate = secretDetails.EndDateTime?.DateTime,
        };

        return result;
    }
}
