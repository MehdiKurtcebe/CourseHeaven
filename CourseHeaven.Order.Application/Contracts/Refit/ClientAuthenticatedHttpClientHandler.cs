using CourseHeaven.Shared.Options;
using Duende.IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;

namespace CourseHeaven.Order.Application.Contracts.Refit;

public class ClientAuthenticatedHttpClientHandler(
    IServiceProvider serviceProvider,
    IHttpClientFactory httpClientFactory) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization is not null)
            return await base.SendAsync(request, cancellationToken);

        using var scope = serviceProvider.CreateScope();
        var identityOptions = scope.ServiceProvider.GetRequiredService<IdentityOptions>();
        var clientSecretOptions = scope.ServiceProvider.GetRequiredService<ClientSecretOptions>();

        var discoveryRequest = new DiscoveryDocumentRequest
        {
            Address = identityOptions.Address,
            Policy = { RequireHttps = false }
        };

        var client = httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(identityOptions.Address);

        var discoveryResponse = await client.GetDiscoveryDocumentAsync(discoveryRequest, cancellationToken);
        if (discoveryResponse.IsError)
            throw new Exception($"Discovery document request failed: {discoveryResponse.Error}");

        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = discoveryResponse.TokenEndpoint,
            ClientId = clientSecretOptions.Id,
            ClientSecret = clientSecretOptions.Secret
        }, cancellationToken);

        if (tokenResponse.IsError) throw new Exception($"Token request failed: {tokenResponse.Error}");

        request.SetBearerToken(tokenResponse.AccessToken!);
        return await base.SendAsync(request, cancellationToken);
    }
}