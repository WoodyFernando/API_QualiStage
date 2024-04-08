using Microsoft.IdentityModel.Tokens;
using Pragma_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Pragma_API.Services
{
    public class TokenService
    {
        public static object GerarToken(Usuario usuario)
        {
            var key = Encoding.ASCII.GetBytes(Key.Secret);
            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                      new Claim(ClaimTypes.Email, usuario.Email),
                      new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                      new Claim("UsuarioID", usuario.Id.ToString()),

                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);

            return new
            {
                token = tokenHandler.WriteToken(token)
            };

        }
    }
}
