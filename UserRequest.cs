using Amazon.CognitoIdentityProvider;

namespace APIStockManager
{
    public class UserRequest
    {
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}
