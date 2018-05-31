using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.DAL
{



    public static class PassWordHelper
    {

        private const int PBKDF2IterCount = 1000;

        private const int PBKDF2SubkeyLength = 32;

        private const int SaltSize = 16;

        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (a == null || b == null || (int)a.Length != (int)b.Length)
            {
                return false;
            }
            bool flag = true;
            for (int i = 0; i < (int)a.Length; i++)
            {
                flag = flag & a[i] == b[i];
            }
            return flag;
        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] bytes;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes rfc2898DeriveByte = new Rfc2898DeriveBytes(password, 16, 1000))
            {
                salt = rfc2898DeriveByte.Salt;
                bytes = rfc2898DeriveByte.GetBytes(32);
            }
            byte[] numArray = new byte[49];
            Buffer.BlockCopy(salt, 0, numArray, 1, 16);
            Buffer.BlockCopy(bytes, 0, numArray, 17, 32);
            return Convert.ToBase64String(numArray);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] bytes;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] numArray = Convert.FromBase64String(hashedPassword);
            if ((int)numArray.Length != 49 || numArray[0] != 0)
            {
                return false;
            }
            byte[] numArray1 = new byte[16];
            Buffer.BlockCopy(numArray, 1, numArray1, 0, 16);
            byte[] numArray2 = new byte[32];
            Buffer.BlockCopy(numArray, 17, numArray2, 0, 32);
            using (Rfc2898DeriveBytes rfc2898DeriveByte = new Rfc2898DeriveBytes(password, numArray1, 1000))
            {
                bytes = rfc2898DeriveByte.GetBytes(32);
            }
            return PassWordHelper.ByteArraysEqual(numArray2, bytes);
        }
    }
}
