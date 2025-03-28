using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using Produtos.Core.Entities;
using Produtos.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Produtos.Core.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtKey = configuration["Jwt:Key"];
            _jwtIssuer = configuration["Jwt:Issuer"];
            _jwtAudience = configuration["Jwt:Audience"];
        }

        public async Task<string> AuthenticateAsync(string login, string password)
        {
            var user = await _userRepository.GetByLogin(login);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Senha))
            {
                throw new Exception("Usuário ou senha inválidos");
            }
            return GenerateJwtToken(user);
        }

        public async Task<bool> RegisterUserAsync(string login, string senha)
        {
            if (await _userRepository.GetByLogin(login) != null)
                return false;

            var senhaCriptografada = BCrypt.Net.BCrypt.HashPassword(senha);
            var user = new User { Login = login, Senha = senhaCriptografada };
            await _userRepository.Add(user);
            return true;
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _jwtIssuer,
                _jwtAudience,
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
