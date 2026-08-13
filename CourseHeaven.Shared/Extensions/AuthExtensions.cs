using System.Security.Claims;
using CourseHeaven.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CourseHeaven.Shared.Extensions;

public static class AuthExtensions
{
    public static IServiceCollection AddAuthenticationAuthorizationExtension(this IServiceCollection services,
        IConfiguration configuration)
    {
        var identityOptions = configuration.GetSection(nameof(IdentityOptions)).Get<IdentityOptions>()!;

        services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.Authority = identityOptions.Address;
            options.Audience = identityOptions.Audience;
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
                RoleClaimType = "roles",
                NameClaimType = "preferred_username"
            };
        }).AddJwtBearer("ClientCredentialScheme", options =>
        {
            options.Authority = identityOptions.Address;
            options.Audience = identityOptions.Audience;
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = true
            };
        });

        services.AddAuthorizationBuilder()
            .AddPolicy("Password", policy =>
            {
                policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(ClaimTypes.Email);
            })
            .AddPolicy("ClientCredential", policy =>
            {
                policy.AuthenticationSchemes.Add("ClientCredentialScheme");
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("client_id");
            });

        return services;
    }
}