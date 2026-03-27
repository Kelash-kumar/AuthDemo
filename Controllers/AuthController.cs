using AuthDemo.DTOs.UserDTOs;
using AuthDemo.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class AuthController(IAuthService service) : BaseApiController
    {
        private readonly IAuthService _authService = service;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {

            var result = await _authService.Login(loginDto);
            return ApiOk(result, "User Logged In Successfully.");

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = _authService.Register(registerDto);
            return ApiOk(result, "User Registered Successfully");

        }
    }
}
