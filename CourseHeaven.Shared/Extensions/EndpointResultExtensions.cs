using System.Net;
using Microsoft.AspNetCore.Http;

namespace CourseHeaven.Shared.Extensions;

public static class EndpointResultExtensions
{
    public static IResult ToGenericResult(this ServiceResult result)
    {
        return result.Status switch
        {
            HttpStatusCode.NoContent => Results.NoContent(),
            HttpStatusCode.NotFound => Results.NotFound(result.Fail),
            _ => Results.Problem(result.Fail!)
        };
    }

    public static IResult ToGenericResult<T>(this ServiceResult<T> result)
    {
        return result.Status switch
        {
            HttpStatusCode.OK => Results.Ok(result),
            HttpStatusCode.Created => Results.Created(result.UrlAsCreated, result),
            HttpStatusCode.NotFound => Results.NotFound(result.Fail),
            _ => Results.Problem(result.Fail!)
        };
    }
}