using Microsoft.AspNetCore.Mvc;

namespace music_api_gateway.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        [HttpPost("register")]
        public IActionResult Register()
        {
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login()
        {
            return Ok();
        }
    }
}
