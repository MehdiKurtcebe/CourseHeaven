using CourseHeaven.Web.Options;
using CourseHeaven.Web.Services;
using Duende.IdentityModel.Client;

namespace CourseHeaven.Web.DelegateHandlers;

public class ClientAuthenticatedHttpClientHandler(
    IdentityOptions identityOptions,
    IHttpContextAccessor httpContextAccessor,
    TokenService tokenService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (httpContextAccessor.HttpContext is null || httpContextAccessor.HttpContext.User.Identity!.IsAuthenticated)
            return await base.SendAsync(request, cancellationToken);

        var tokenResponse = await tokenService.GetClientAccessTokenAsync(cancellationToken);
        if (tokenResponse.IsError)
            throw new UnauthorizedAccessException($"Failed to retrieve client access token: {tokenResponse.Error}");

        request.SetBearerToken(tokenResponse.AccessToken!);
        return await base.SendAsync(request, cancellationToken);
    }
}