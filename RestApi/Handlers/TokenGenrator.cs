using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RestApi.Handlers
{
    public static class TokenGenerator
    {
        /// <summary>
        /// Refresh Token 생성 (길이 제한 없음)
        /// </summary>
        public static string GenerateRefreshToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            // Base64 → URL-safe 변환
            var token = Convert.ToBase64String(randomNumber)
                .Replace('+', '-') // URL-safe
                .Replace('/', '_')
                .TrimEnd('=');

            // 길이 제한 제거 → 그대로 반환
            return token;
        }

        /// <summary>
        /// Access Token 생성 (JWT)
        /// </summary>
        public static string GenerateAccessToken(string userId, string clientIp)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("MySuperSecretKey1234567890123456789");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userId),
                    new Claim("role", "Admin"),
                    new Claim("ip", clientIp)
                }),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
                // Expires 생략 → 영구 토큰
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
