using System.Text.Json;
using Refit;

namespace CourseHeaven.Web.Extensions;

public static class ProblemDetailsExtensions
{
    public static void LogProblemDetailsExtension(this ILogger logger, ApiException? apiException)
    {
        if (string.IsNullOrEmpty(apiException!.Content))
        {
            logger.LogError(apiException.Message);
            return;
        }
        
        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(apiException.Content);
        if (problemDetails is null) return;
        
        logger.LogError("Problem Details: Title: {Title}, Detail: {Detail}, Status: {Status}",
            problemDetails.Title, problemDetails.Detail, problemDetails.Status);
    }
}