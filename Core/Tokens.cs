using System;
using System.Text;
using System.Security;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Honeymustard
{
    public class Tokens
    {
        public string Issuer { get; set; }
        public string Secret { get; set; }

        public DateTime Expires
        {
            get => DateTime.UtcNow.AddMinutes(30);
        }

        public SymmetricSecurityKey SigningKey
        {
            get => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
        }

        public SigningCredentials SigningCredentials
        {
            get => new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);
        }

        public string Token
        {
            get => new JwtSecurityTokenHandler().WriteToken(GenerateToken());
        }

        public TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidIssuer = Issuer,
                IssuerSigningKey = SigningKey,
            };
        }

        public JwtSecurityToken GenerateToken()
        {
            return new JwtSecurityToken(
                issuer: Issuer,
                expires: Expires,
                signingCredentials: SigningCredentials
            );
        }
    }
}