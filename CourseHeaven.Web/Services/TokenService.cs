using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CourseHeaven.Web.Options;
using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace CourseHeaven.Web.Services;

public class TokenService(HttpClient client, IdentityOptions identityOptions)
{
    public List<Claim> ExtractClaim(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        return jwtToken.Claims.ToList();
    }

    public AuthenticationProperties CreateAuthenticationProperties(TokenResponse tokenResponse)
    {
        var authenticationTokens = new List<AuthenticationToken>
        {
            new()
            {
                Name = OpenIdConnectParameterNames.AccessToken,
                Value = tokenResponse.AccessToken!
            },
            new()
            {
                Name = OpenIdConnectParameterNames.RefreshToken,
                Value = tokenResponse.RefreshToken!
            },
            new()
            {
                Name = OpenIdConnectParameterNames.ExpiresIn,
                Value = DateTimeOffset.UtcNow.AddSeconds(tokenResponse.ExpiresIn).ToString("o")
            }
        };

        AuthenticationProperties authenticationProperties = new()
        {
            IsPersistent = true
        };
        authenticationProperties.StoreTokens(authenticationTokens);

        return authenticationProperties;
    }

    public async Task<TokenResponse> GetTokensByRefreshTokenAsync(string refreshToken,
        CancellationToken cancellationToken)
    {
        var discoveryDocument = new DiscoveryDocumentRequest
        {
            Address = identityOptions.Address,
            Policy = { RequireHttps = false }
        };

        client.BaseAddress = new Uri(identityOptions.Address);
        var discoveryResponse = await client.GetDiscoveryDocumentAsync(discoveryDocument, cancellationToken);
        if (discoveryResponse.IsError)
            throw new Exception($"Error retrieving discovery document: {discoveryResponse.Error}");

        var tokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
        {
            Address = discoveryResponse.TokenEndpoint,
            ClientId = identityOptions.Web.ClientId,
            ClientSecret = identityOptions.Web.ClientSecret,
            RefreshToken = refreshToken
        }, cancellationToken);

        return tokenResponse;
    }

    public async Task<TokenResponse> GetClientAccessTokenAsync(CancellationToken cancellationToken)
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

        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = discoveryResponse.TokenEndpoint,
            ClientId = identityOptions.Web.ClientId,
            ClientSecret = identityOptions.Web.ClientSecret
        }, cancellationToken);

        return tokenResponse.IsError
            ? throw new Exception($"Error retrieving access token: {tokenResponse.Error}")
            : tokenResponse;
    }
}