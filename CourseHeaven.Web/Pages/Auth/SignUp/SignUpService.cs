using System.Net;
using CourseHeaven.Web.Options;
using CourseHeaven.Web.Services;
using Duende.IdentityModel.Client;

namespace CourseHeaven.Web.Pages.Auth.SignUp;

public record KeyCloakErrorResponse(string ErrorMessage);

public class SignUpService(IdentityOptions identityOptions, HttpClient client, ILogger<SignUpService> logger)
{
    
    public async Task<ServiceResult> CreateAccountAsync(SignUpViewModel model, CancellationToken cancellationToken)
    {
        var token = await GetClientCredentialTokenAsAdminAsync(cancellationToken);
        client.SetBearerToken(token);
        
        var address = $"{identityOptions.BaseAddress}/admin/realms/{identityOptions.Realm}/users";
        var userCreateRequest = CreateUserCreateRequest(model);

        var response = await client.PostAsJsonAsync(address, userCreateRequest, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode != HttpStatusCode.InternalServerError)
            {
                var keyCloakErrorResponse = await response.Content.ReadFromJsonAsync<KeyCloakErrorResponse>(cancellationToken);
                return ServiceResult.Error(keyCloakErrorResponse!.ErrorMessage);
            }
            
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogError(error);

            return ServiceResult.Error("System error occurred while creating the account. Please try again later.");
        }
        
        return ServiceResult.Success();
    }
    
    private static UserCreateRequest CreateUserCreateRequest(SignUpViewModel model)
    {
        return new UserCreateRequest(
            model.Username,
            true,
            model.FirstName,
            model.LastName,
            model.Email,
            [new Credential("password", model.Password, false)]);
    }
    
    private async Task<string> GetClientCredentialTokenAsAdminAsync(CancellationToken cancellationToken)
    {
        var discoveryRequest = new DiscoveryDocumentRequest()
        {
            Address = identityOptions.Address,
            Policy = { RequireHttps = false }
        };

        client.BaseAddress = new Uri(identityOptions.Address);
        var discoveryResponse = await client.GetDiscoveryDocumentAsync(discoveryRequest, cancellationToken);
        if (discoveryResponse.IsError)
        {
            throw new Exception($"Discovery document request failed: {discoveryResponse.Error}");
        }

        var tokenResponse = await client.RequestClientCredentialsTokenAsync(
            new ClientCredentialsTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientId = identityOptions.Admin.ClientId,
                ClientSecret = identityOptions.Admin.ClientSecret,
            },
            cancellationToken);
        if (tokenResponse.IsError)
        {
            throw new Exception($"Token request failed: {tokenResponse.Error}");
        }

        return tokenResponse.AccessToken!;
    }
}