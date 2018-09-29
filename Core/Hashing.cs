using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Honeymustard
{
    public class Hashing
    {
        /// <summary>
        /// Generates a new random salt.
        /// </summary>
        /// <returns>Returns the salt as a base64 encoded string.</returns>
        public static string GenerateSalt()
        {
            byte[] salt = new byte[128/8];
            RandomNumberGenerator.Create().GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Generates a new Pbkdf2 hash.
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <param name="salt">A base64 encoded salt</param>
        /// <returns>Returns a base64 encoded hash.</returns>
        public static string GenerateHash(string password, string salt)
        {
            var bytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            );

            return Convert.ToBase64String(bytes);
        }
    }
}