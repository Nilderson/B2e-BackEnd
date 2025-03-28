using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Produtos.Core.Entities;
using Produtos.Core.Services;
using Produtos.Infra.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _1.Produtos.Auth.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var success = await _userService.RegisterUserAsync(user.Login, user.Senha);
            if (!success) return BadRequest("Usuário já existe.");
            return Ok("Usuário cadastrado com sucesso.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var token = await _userService.AuthenticateAsync(user.Login, user.Senha);
            if (token == null) 
                return Unauthorized("Login ou senha inválidos.");

            return Ok(new { Token = token });
        }

    }
}
