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
        var result = await GraphHelper.GetSecretExpirationDateAsync(secretId);
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
