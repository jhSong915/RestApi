using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestApi.Models
{
    public class RefreshToken
    {
        [Key]
        [MaxLength(512)]
        public string Token { get; set; }

        public string UserId { get; set; }

        public DateTime Expiration { get; set; }
    }

}