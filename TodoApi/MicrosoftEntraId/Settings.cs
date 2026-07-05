using System;

using Microsoft.Extensions.Configuration;

namespace TodoApi.MicrosoftEntraId;

public class Settings
{
    public Guid? TenantId { get; set; }

    public Guid? ClientId { get; set; }

    public Guid? ClientSecretId { get; set; }

    public string? ClientSecret { get; set; }

    public static Settings LoadSettings()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.Development.json", optional: true)
            .AddUserSecrets<Program>()
            .Build();

        var result = config.GetRequiredSection("MicrosoftEntraID").Get<Settings>();

        if (result == null)
        {
            throw new Exception("Could not load app settings. See README for configuration instructions.");
        }

        return result;
    }
}
