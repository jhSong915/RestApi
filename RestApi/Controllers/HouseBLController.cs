using Oracle.ManagedDataAccess.Client;
using RestApi.Data;
using RestApi.Handlers;
using RestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RestApi.Controllers
{
    public class HouseBLController : ApiController
    {
        [Authorize]
        [HttpPost]
        [Route("api/housebl")]
        public IHttpActionResult GetHouseInfo([FromBody] HouseRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.BLNo)
                                || string.IsNullOrWhiteSpace(request.CustomsCode)
                                || string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest("BL 번호, 세관코드, Refresh Token은 필수입니다.");
            }

            string userId;
            string newRefreshToken;
            if (!ValidateAndRefreshToken(request.RefreshToken, out userId, out newRefreshToken))
            {
                return Unauthorized();
            }

            // ✅ 세관코드 등으로 연결 문자열 선택
            var oracleConnStr = GetOracleConnectionString(request.CustomsCode);

            using (var dbContext = new OracleDbContext(oracleConnStr))
            {
                var houseInfo = new HouseInfoHandler(dbContext);
                var houses = houseInfo.GetHouseList(request.BLNo, request.CustomsCode);

                if (houses == null || houses.Count == 0)
                {
                    return NotFound();
                }

                return Ok(new
                {
                    refresh_token = newRefreshToken,
                    data = houses
                });
            }
        }

        private string GetOracleConnectionString(string customsCode)
        {
            // ✅ 세관코드별 연결 문자열 매핑
            switch (customsCode)
            {
                case "010": // 예시: 인천세관
                    return "User Id=xxx;Password=xxx;Data Source=host:1521/service";
                case "020": // 예시: 부산세관
                    return "User Id=yyy;Password=yyy;Data Source=host:1521/service";
                default:
                    throw new Exception("지원하지 않는 세관 코드입니다.");
            }
        }

        private bool ValidateAndRefreshToken(string refreshToken, out string userId, out string newRefreshToken)
        {
            userId = null;
            newRefreshToken = refreshToken;

            using (var db = new AuthDbContext())
            {
                var tokenEntry = db.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
                if (tokenEntry == null || tokenEntry.Expiration < DateTime.UtcNow)
                    return false;

                userId = tokenEntry.UserId;
                var remaining = tokenEntry.Expiration - DateTime.UtcNow;

                if (remaining.TotalHours <= 1)
                {
                    // ✅ 만료된 토큰 삭제
                    var expiredTokens = db.RefreshTokens.Where(r => r.Expiration < DateTime.UtcNow);
                    db.RefreshTokens.RemoveRange(expiredTokens);

                    // ✅ 해당 유저 토큰 삭제
                    var userTokens = db.RefreshTokens.Where(r => r.UserId == tokenEntry.UserId);
                    db.RefreshTokens.RemoveRange(userTokens);

                    // ✅ 새 토큰 발급
                    newRefreshToken = TokenGenerator.GenerateRefreshToken();
                    db.RefreshTokens.Add(new RefreshToken
                    {
                        Token = newRefreshToken,
                        UserId = userId,
                        Expiration = DateTime.UtcNow.AddDays(1)
                    });

                    db.SaveChanges();
                }
            }
            return true;
        }
    }
}
