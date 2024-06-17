using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rino.Domain.Entities;
using Rino.Domain.Handlers;

namespace Rino.API.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserHandler _userHandler;

        public AuthController(UserHandler userHandler)
        {
            _userHandler = userHandler;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateUserLogin([FromBody] LoginCommand loginCommand)
        {
            var user = await _userHandler.Authenticate(loginCommand);

            if (user == null)
                return Unauthorized();

            return Ok(new { user });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] UserCreateCommand registerCommand)
        {
            var result = await _userHandler.RegisterUser(registerCommand);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

    }
}
