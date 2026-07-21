using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonsApp.Dtos.Auth;
using PersonsApp.Dtos.Common;
using PersonsApp.Services.Auth;

namespace PersonsApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authservice;
        public AuthController(IAuthService authService)
        {
            _authservice = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDto<LoginResponseDto>>> Login(LoginDto dto)
        {
            var response = await _authservice.LoginAsync(dto);

            return StatusCode(response.StatusCode, new ResponseDto<LoginResponseDto>
            {
                Status = response.Status,
                Message = response.Message,
                Data = response.Data
            });
        }
    }
}