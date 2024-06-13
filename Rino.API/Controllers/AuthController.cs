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
        public async Task<IActionResult> RegisterUser([FromBody] RegisterCommand registerCommand)
        {
            var result = await _userHandler.Register(registerCommand);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand updateProfileCommand)
        {
            // Obter o nome do usuário atualmente autenticado
            var userName = User.Identity.Name;

            var result = await _userHandler.UpdateProfile(userName, updateProfileCommand);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand changePasswordCommand)
        {
            // Obter o nome do usuário atualmente autenticado
            var userName = User.Identity.Name;

            var result = await _userHandler.ChangePassword(userName, changePasswordCommand);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand forgotPasswordCommand)
        {
            var result = await _userHandler.ForgotPassword(forgotPasswordCommand.Email);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand resetPasswordCommand)
        {
            var result = await _userHandler.ResetPassword(resetPasswordCommand);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            // Obter o nome do usuário atualmente autenticado
            var userName = User.Identity.Name;

            var user = await _userHandler.GetUserInfo(userName);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }
    }
}
