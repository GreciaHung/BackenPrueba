using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public class PasswordHasherService
    {
        private const int SaltValueSize = 8;
        private const int OutputSize = 32;

        public static byte[] GenerateSaltValue()
        {
            var rngCsp = new RNGCryptoServiceProvider();
            var randomNumber = new byte[SaltValueSize];

            // Fill the array with a random value.
            rngCsp.GetBytes(randomNumber);

            return randomNumber;
        }

        public static string GenerateSalt()
        {
            return Convert.ToBase64String(GenerateSaltValue());
        }

        public static string HashPassword(string clearData, byte[] saltValue)
        {
            if (saltValue == null)
            {
                throw new InvalidOperationException("You need a salt to hash password securely");
            }

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(clearData, saltValue);

            var hashedPassword = pbkdf2.GetBytes(OutputSize);

            return Convert.ToBase64String(hashedPassword);
        }

        public static string HashPassword(string clearData, string salt)
        {
            if (String.IsNullOrWhiteSpace(salt))
            {
                throw new InvalidOperationException("You need a salt to hash password securely");
            }

            return HashPassword(clearData, Convert.FromBase64String(salt));
        }
    }
}
