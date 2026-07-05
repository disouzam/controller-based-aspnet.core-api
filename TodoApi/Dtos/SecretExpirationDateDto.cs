using System;

namespace TodoApi.Dtos;

public class SecretExpirationDateDto
{
    public Guid SecretId { get; set; }

    public DateTime? ExpirationDate { get; set; }
}
