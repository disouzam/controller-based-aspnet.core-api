using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TodoApi.Dtos;
using TodoApi.MicrosoftGraph;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SecretExpirationDateController : ControllerBase
{
    // GET: api/SecretExpirationDate/5
    [HttpGet("{secretId}")]
    public async Task<ActionResult<SecretExpirationDateDto>> GetSecretExpirationDate(Guid secretId)
    {
        var accessToken = await GetAccessTokenAsync();

        var result = new SecretExpirationDateDto
        {
            SecretId = secretId,
            ExpirationDate = DateTime.UtcNow.AddDays(30) // Example expiration date
        };

        return Ok(result);
    }


    private async Task<string> GetAccessTokenAsync()
    {
        try
        {
            var appOnlyToken = await GraphHelper.GetAppOnlyTokenAsync();
            return appOnlyToken;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting app-only access token: {ex.Message}");
            throw;
        }
    }
}
