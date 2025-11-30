using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RestApi.Models
{
    public class HouseRequest
    {
        public string BLNo { get; set; }          // B/L 번호
        public string CustomsCode { get; set; }   // 세관코드
        public string RefreshToken { get; set; }  // Refresh Token
    }
}