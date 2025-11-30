using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RestApi.Handlers
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            // bcrypt 해시 생성 (WorkFactor=12 권장)
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}