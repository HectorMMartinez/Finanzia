using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Finanzia.Domain.Entities;
using System.Text;
using Finanzia.Application.Contract;

namespace Finanzia.Application.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        public string GenerarToken(Usuario usuario)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, usuario.NombreCompleto),
            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKey"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7291",
                audience: "https://localhost:7291",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}