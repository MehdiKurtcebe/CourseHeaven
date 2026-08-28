using System.Net;
using System.Security.Claims;
using CourseHeaven.Web.Services;
using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace CourseHeaven.Web.DelegateHandlers;

public class AuthenticatedHttpClientHandler(IHttpContextAccessor httpContextAccessor, TokenService tokenService)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (httpContextAccessor.HttpContext is null) return await base.SendAsync(request, cancellationToken);

        var user = httpContextAccessor.HttpContext.User;
        if (!user.Identity!.IsAuthenticated) return await base.SendAsync(request, cancellationToken);

        var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
        if (string.IsNullOrEmpty(accessToken))
            throw new UnauthorizedAccessException("No access token found.");

        request.SetBearerToken(accessToken);

        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode != HttpStatusCode.Unauthorized) return response;

        var refreshToken =
            await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
        if (string.IsNullOrEmpty(refreshToken))
            throw new UnauthorizedAccessException("No refresh token found.");

        var tokenResponse = await tokenService.GetTokensByRefreshTokenAsync(refreshToken, cancellationToken);
        if (tokenResponse.IsError)
            throw new UnauthorizedAccessException("Failed to refresh access token.");

        var userClaims = httpContextAccessor.HttpContext.User.Claims;
        var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme,
            ClaimTypes.Name, ClaimTypes.Role);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authProperties = tokenService.CreateAuthenticationProperties(tokenResponse);
        await httpContextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal, authProperties);

        request.SetBearerToken(tokenResponse.AccessToken!);
        return await base.SendAsync(request, cancellationToken);
    }
}