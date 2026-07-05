using System;
using System.Net;
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
        try
        {
            var result = await GraphHelper.GetSecretExpirationDateAsync(secretId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, $"Internal server error: {ex.Message}");
        }
        
    }
}
