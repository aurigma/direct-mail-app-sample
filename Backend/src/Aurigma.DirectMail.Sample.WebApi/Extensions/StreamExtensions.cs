using System.IO;

namespace Aurigma.DirectMail.Sample.WebApi.Extensions;

public static class StreamExtensions
{
    public static byte[] ToByteArray(this Stream stream)
    {
        if (stream is MemoryStream ms)
        {
            return ms.ToArray();
        }

        using (var msTemp = new MemoryStream())
        {
            stream.CopyTo(msTemp);
            return msTemp.ToArray();
        }
    }
}
