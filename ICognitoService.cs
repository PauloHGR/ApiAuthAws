using Amazon.CognitoIdentityProvider.Model;

namespace APIStockManager
{
    public interface ICognitoService
    {
        Task<InitiateAuthResponse> SignInUserAsyncWithPasswordAuth(string username, string password);
    }
}