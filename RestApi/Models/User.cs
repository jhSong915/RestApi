using System.ComponentModel.DataAnnotations;

namespace RestApi.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; }
        public string PasswordHash { get; set; } // ✅ bcrypt 해시 저장
    }
}
