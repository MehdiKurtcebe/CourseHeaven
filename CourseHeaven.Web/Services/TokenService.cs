using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace CourseHeaven.Web.Services;

public class TokenService
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
}