using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace InterceptorOAuthToNTLM.TempItems
{
    public class Gandalf5000
    {
        private const string _validIssuer = "GandalfTheGray";
        private const string _validAudience = "TheHobbits";
        private const string _securityKey = "I am a servant of the Secret Fire, wielder of the flame of Anor. You cannot pass.";

        public string GenerateJwtToken(string clientId, string clientSecret, string scope)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, clientId),
                new Claim("client_secret", clientSecret),
                new Claim("scope", scope)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _validIssuer,
                audience: _validAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateJwtToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _validIssuer,
                ValidAudience = _validAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            try
            {
                tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
            }
            catch
            {
                return false;
            }

            return validatedToken != null;
        }

    }
}
