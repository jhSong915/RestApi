using BCrypt.Net; // BCrypt.Net-Next 패키지 필요 (NuGet)
using RestApi.Data;
using RestApi.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace RestApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly AuthDbContext _db = new AuthDbContext();

        // POST api/users/register
        [HttpPost]
        [Route("api/user/register")]
        public IHttpActionResult Register([FromBody] User request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.PasswordHash))
                return BadRequest("UserId와 Password는 필수입니다.");

            // 이미 존재하는지 확인
            if (_db.Users.Any(u => u.UserId == request.UserId))
                return Conflict(); // 409

            // bcrypt 해시 생성
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);

            var user = new User
            {
                UserId = request.UserId,
                PasswordHash = passwordHash
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            return Ok(new { message = "사용자 등록 완료", userId = user.UserId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
