using System;

namespace Aurigma.DirectMail.Sample.DomainEntities.Entities.Editor;

public class Token
{
    public string UserId { get; set; }

    public string TokenId { get; set; }

    public DateTimeOffset ExpireTime { get; set; }

    public int OriginalSeconds { get; set; }

    public bool UpdateOnCall { get; set; }

    public object UserData { get; set; }
}
