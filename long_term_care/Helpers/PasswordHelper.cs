using System;
using System.Security.Cryptography;
using System.Text;

namespace long_term_care.Helpers
{
    public static class PasswordHelper
    {
        public static string ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public static bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            string inputPasswordHash = ComputeSHA256Hash(inputPassword);
            return inputPasswordHash.Equals(hashedPassword);
        }

       
    }
}
