using System;
using System.Linq;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Helpers
{
    public class RandomStr
    {
        private static Random random = new Random();

        /// <summary>
        /// Generate random alphanumeric string
        /// </summary>
        /// <param name="length">String length</param>
        /// <returns>New string of length size</returns>
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
