using System;
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

        appClient ??= new GraphServiceClient(
                clientSecretCredential,
                /* Use the default scope, which will request the scopes
                   configured on the app registration */
                ["https://graph.microsoft.com/.default"]);
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

        var application = appClient.Applications[appId];

        try
        {
            var appDetails = await application.GetAsync();
        }
        catch (Exception)
        {
            throw new Exception($"Error retrieving application details for App ID: {appId}");
        }

        // Example implementation, replace with actual Graph API call
        var result = new SecretExpirationDateDto
        {
            SecretId = secretId,
            ExpirationDate = DateTime.UtcNow.AddDays(30) // Example expiration date
        };

        return result;
    }
}
