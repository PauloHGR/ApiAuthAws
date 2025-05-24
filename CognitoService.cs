using Amazon.AspNetCore.Identity.Cognito;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace APIStockManager
{
    public class CognitoService : ICognitoService
    {
        private readonly IAmazonCognitoIdentityProvider _cognitoClient;
        private readonly ILogger<CognitoService> _logger;
        private readonly string _clientId;
        private readonly string _clientSecret;
        public CognitoService(
            IAmazonCognitoIdentityProvider cognitoClient,
            ILogger<CognitoService> logger,
            IOptions<IdentityProviderConfiguration> options)
        {
            _cognitoClient = cognitoClient;
            _logger = logger;
            _clientId = options.Value.ClientId;
            _clientSecret = options.Value.ClientSecret;
        }

        private string CalculateSecretHash(string username)
        {
            var key = Encoding.UTF8.GetBytes(_clientSecret);
            using (var hmac = new HMACSHA256(key))
            {
                var message = Encoding.UTF8.GetBytes(username + _clientId);
                var hash = hmac.ComputeHash(message);
                return Convert.ToBase64String(hash);
            }
        }

        public async Task<InitiateAuthResponse> SignInUserAsyncWithPasswordAuth(string username, string password)
        {
            var authRequest = new InitiateAuthRequest
            {
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                ClientId = _clientId,
                AuthParameters = new Dictionary<string, string>
        {
            { "USERNAME", username },
            { "PASSWORD", password },
            { "SECRET_HASH", CalculateSecretHash(username) }
        }
            };

            try
            {
                var authResponse = await _cognitoClient.InitiateAuthAsync(authRequest);
                return authResponse;
            }
            catch (NotAuthorizedException)
            {
                Console.WriteLine("The username or password is incorrect.");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
    }
}
