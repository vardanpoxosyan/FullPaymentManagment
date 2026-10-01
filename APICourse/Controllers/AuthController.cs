using APICourse.DTO;
using APICourse.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO,CancellationToken cancellationToken)
        {
             var response = await authService.RegisterAsync(registerDTO,cancellationToken);
            return Ok(response);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO,CancellationToken cancellationToken)
        {
            var login = await authService.LoginAsync(loginDTO, cancellationToken);
            return Ok(login);
        }
    }
}
