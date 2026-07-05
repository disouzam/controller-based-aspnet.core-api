using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TodoApi.Dtos;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SecretExpirationDateController : ControllerBase
{
    // GET: api/SecretExpirationDate/5
    [HttpGet("{secretId}")]
    public async Task<ActionResult<SecretExpirationDateDto>> GetSecretExpirationDate(Guid secretId)
    {
        var result = new SecretExpirationDateDto
        {
            SecretId = secretId,
            ExpirationDate = DateTime.UtcNow.AddDays(30) // Example expiration date
        };

        return Ok(result);
    }
}
