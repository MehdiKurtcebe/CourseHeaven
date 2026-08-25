using Microsoft.AspNetCore.Diagnostics;

namespace CourseHeaven.Web.ExceptionHandlers;

public class UnauthorizedAccessExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not UnauthorizedAccessException)
            return ValueTask.FromResult(false);

        httpContext.Response.Redirect("/Auth/SignIn");
        return ValueTask.FromResult(true);
    }
}