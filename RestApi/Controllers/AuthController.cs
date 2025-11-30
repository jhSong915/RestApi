using Microsoft.IdentityModel.Tokens;
using RestApi.Data;
using RestApi.Models;
using RestApi.Handlers;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Data.Entity.Validation;

namespace RestApi.Controllers
{
    public class AuthController : ApiController
    {
        [HttpPost]
        [Route("api/auth/login")]
        public IHttpActionResult Login(UserCredential login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.UserId) || string.IsNullOrWhiteSpace(login.PrivateKey))
                return BadRequest("UserId와 비밀번호는 필수입니다.");

            using (var db = new AuthDbContext())
            {
                var user = db.Users.SingleOrDefault(u => u.UserId == login.UserId);

                if (user == null || !PasswordHasher.VerifyPassword(login.PrivateKey, user.PasswordHash))
                    return Unauthorized();

                var clientIp = ((HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;

                // Access Token 확인 또는 생성
                var accessTokenEntry = db.AccessTokens.SingleOrDefault(t => t.UserId == user.UserId && !t.IsRevoked);

                if (accessTokenEntry == null)
                {
                    accessTokenEntry = new AccessToken
                    {
                        Token = TokenGenerator.GenerateAccessToken(user.UserId, clientIp),
                        UserId = user.UserId,
                        IpAddress = clientIp,
                        IssuedAt = DateTime.UtcNow,
                        IsRevoked = false
                    };
                    db.AccessTokens.Add(accessTokenEntry);
                }

                            // Refresh Token 생성
                var refreshToken = TokenGenerator.GenerateRefreshToken();
                db.RefreshTokens.Add(new RefreshToken
                {
                    Token = refreshToken,
                    UserId = user.UserId,
                    Expiration = DateTime.UtcNow.AddDays(1)
                });

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var errors = ex.EntityValidationErrors
                        .SelectMany(e => e.ValidationErrors)
                        .Select(e => $"{e.PropertyName}: {e.ErrorMessage}");
                    return InternalServerError(new Exception("유효성 검사 실패: " + string.Join(", ", errors)));
                }

                return Ok(new
                {
                    access_token = accessTokenEntry.Token,
                    refresh_token = refreshToken
                });
            }
        }

        [HttpPost]
        [Route("api/auth/refresh")]
        public IHttpActionResult RefreshToken([FromBody] RefreshRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.RefreshToken))
                return BadRequest("Refresh Token이 필요합니다.");

            using (var db = new AuthDbContext())
            {
                var tokenEntry = db.RefreshTokens
                    .FirstOrDefault(t => t.Token == request.RefreshToken);

                if (tokenEntry == null || tokenEntry.Expiration < DateTime.UtcNow)
                {
                    return Unauthorized();
                }

                var accessTokenEntry = db.AccessTokens
                    .Where(t => t.UserId == tokenEntry.UserId && !t.IsRevoked)
                    .FirstOrDefault();

                if (accessTokenEntry == null)
                    return Unauthorized();

                string newRefreshToken = tokenEntry.Token; // 기본은 기존 토큰 유지

                            // ⏱ 만료까지 1시간 이하 남았을 때만 새로 발급
                var remaining = tokenEntry.Expiration - DateTime.UtcNow;
                if (remaining.TotalHours <= 1)
                {
                                    // ✅ 모든 유저의 만료된 Refresh Token 삭제
                    var expiredTokens = db.RefreshTokens
                        .Where(r => r.Expiration < DateTime.UtcNow);
                    db.RefreshTokens.RemoveRange(expiredTokens);

                                    // ✅ 호출한 유저의 기존 Refresh Token도 삭제
                    db.RefreshTokens.RemoveRange(
                        db.RefreshTokens.Where(r => r.UserId == tokenEntry.UserId)
                    );

                                     // 새 토큰 발급 (1일짜리)
                    newRefreshToken = TokenGenerator.GenerateRefreshToken();
                    db.RefreshTokens.Add(new RefreshToken
                    {
                        Token = newRefreshToken,
                        UserId = tokenEntry.UserId,
                        Expiration = DateTime.UtcNow.AddDays(1) // ✅ 항상 1일짜리로 발급
                    });

                    db.SaveChanges();
                }

                return Ok(new
                {
                    access_token = accessTokenEntry.Token,
                    refresh_token = newRefreshToken
                });
            }
        }
    }
}