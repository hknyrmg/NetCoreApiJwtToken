using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TokenBasedAuth_NetCore.Utils
{
    public class HashUtil
    {
        public static string GetSha256FromString(string password)
        {
            var message = Encoding.ASCII.GetBytes(password);
            var hashString = new SHA256Managed();

            var hashValue = hashString.ComputeHash(message);
            return hashValue.Aggregate("", (current, x) => current + $"{x:x2}");
        }
    }
}
