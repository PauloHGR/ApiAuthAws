using Microsoft.AspNetCore.Mvc;

namespace APIStockManager
{
    [ApiController]
    public class CognitoController : ControllerBase
    {
        private readonly ICognitoService _cognitoService;
        public CognitoController(ICognitoService cognitoService)
        {
            _cognitoService = cognitoService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> SignIn([FromBody] UserRequest request)
        {
            try
            {
                var authResponse = await _cognitoService.SignInUserAsyncWithPasswordAuth(request.Username, request.Password);
                return Ok(authResponse.AuthenticationResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
