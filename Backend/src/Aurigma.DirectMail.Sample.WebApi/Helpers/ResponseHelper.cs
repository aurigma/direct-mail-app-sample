using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.WebApi.Dtos.Response;

namespace Aurigma.DirectMail.Sample.WebApi.Helpers;

public static class ResponseHelper
{
    public static ConflictDto BuildConflictDto(InvalidStateAppException ex)
    {
        return new ConflictDto
        {
            PropertyName = ex.PropertyName?.ToLower(),
            PropertyValue = ex.PropertyValue,
            Description = ex.Message,
        };
    }
}
