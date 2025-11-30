using System;
using System.ComponentModel.DataAnnotations;

namespace RestApi.Models
{
    public class AccessToken
    {
        [Key]
        [MaxLength(512)]
        public string Token { get; set; }       // JWT 문자열
        public string UserId { get; set; }      // 사용자 ID
        public string IpAddress { get; set; }   // 발급 당시 IP
        public DateTime IssuedAt { get; set; }  // 발급 시각
        public bool IsRevoked { get; set; }     // 로그아웃/무효화 여부
    }
}
