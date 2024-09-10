using System;

namespace Aurigma.DirectMail.Sample.WebApi.Dtos.Editor;

public class TokenDto
{
    public string TokenId { get; set; }

    public DateTimeOffset ExpireTime { get; set; }

    public int OriginalSeconds { get; set; }
}
