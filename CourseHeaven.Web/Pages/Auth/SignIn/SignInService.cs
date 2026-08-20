using System.Security.Claims;
using CourseHeaven.Web.Options;
using CourseHeaven.Web.Services;
using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CourseHeaven.Web.Pages.Auth.SignIn;

public class SignInService(
    IHttpContextAccessor contextAccessor,
    TokenService tokenService,
    IdentityOptions identityOptions,
    HttpClient client,
    ILogger<SignInService> logger)
{
    public async Task<ServiceResult> AuthenticateAsync(SignInViewModel model, CancellationToken cancellationToken)
    {
        var tokenResponse = await GetAccessTokenAsync(model, cancellationToken);
        if (tokenResponse.IsError)
            return ServiceResult.Error(tokenResponse.Error!, tokenResponse.ErrorDescription!);

        var userClaims = tokenService.ExtractClaim(tokenResponse.AccessToken!);
        var claimIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme,
            ClaimTypes.Name, ClaimTypes.Role);
        var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
        var authenticationProperties = tokenService.CreateAuthenticationProperties(tokenResponse);

        await contextAccessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal, authenticationProperties);

        return ServiceResult.Success();
    }

    private async Task<TokenResponse> GetAccessTokenAsync(SignInViewModel model, CancellationToken cancellationToken)
    {
        var discoveryRequest = new DiscoveryDocumentRequest
        {
            Address = identityOptions.Address,
            Policy = { RequireHttps = false }
        };

        client.BaseAddress = new Uri(identityOptions.Address);
        var discoveryResponse = await client.GetDiscoveryDocumentAsync(discoveryRequest, cancellationToken);
        if (discoveryResponse.IsError)
            throw new Exception($"Error retrieving discovery document: {discoveryResponse.Error}");

        var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
        {
            Address = discoveryResponse.TokenEndpoint,
            ClientId = identityOptions.Web.ClientId,
            ClientSecret = identityOptions.Web.ClientSecret,
            UserName = model.Email,
            Password = model.Password
        }, cancellationToken);

        return tokenResponse;
    }
}