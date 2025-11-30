using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.IdentityModel.Tokens;
using RestApi.Data;   // ✅ DBContext 참조
using RestApi.Models; // ✅ AccessToken 모델 참조

namespace RestApi.Handlers
{
    public class JwtValidationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains("Authorization"))
                return await base.SendAsync(request, cancellationToken);

            var token = request.Headers.Authorization?.Parameter;
            if (string.IsNullOrEmpty(token))
                return request.CreateResponse(HttpStatusCode.Unauthorized, "Missing token");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("MySuperSecretKey1234567890123456789");

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                var principal = new ClaimsPrincipal(identity);

                // ✅ DB에서 Access Token 상태 확인
                using (var db = new AuthDbContext())
                {
                    var tokenEntry = db.AccessTokens.SingleOrDefault(t => t.Token == token);

                    if (tokenEntry == null || tokenEntry.IsRevoked)
                        return request.CreateResponse(HttpStatusCode.Unauthorized, "Token revoked or not found");
                }

                // ✅ IP Claim 검증
                var tokenIp = jwtToken.Claims.FirstOrDefault(c => c.Type == "ip")?.Value;
                var requestIp = ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;

                if (tokenIp != requestIp)
                {
                    return request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid IP for this token");
                }

                Thread.CurrentPrincipal = principal;
                if (HttpContext.Current != null)
                    HttpContext.Current.User = principal;
            }
            catch (Exception ex)
            {
                Console.WriteLine("JWT 검증 실패: " + ex.Message);
                return request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid token");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}