using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestApi.Models
{
    public class UserCredential
    {
        public string UserId { get; set; }
        public string PrivateKey { get; set; }
    }
}