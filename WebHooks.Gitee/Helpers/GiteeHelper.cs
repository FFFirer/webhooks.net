using System.Security.Cryptography;
using System.Text;

namespace WebHooks.Gitee.Helpers
{
    public class GiteeHelper
    {
        /// <summary>
        /// Gitee签名校验
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="secret"></param>
        /// <return>
        /// sign
        /// </return>
        public static byte[] CalcGiteeSign(string timestamp, string secret)
        {
            ValidateTimestamp(timestamp);

            var plain = $"{timestamp}\n{secret}";

            // HmacSHA256
            var secretBytes = Encoding.UTF8.GetBytes(secret);
            var plainBytes = Encoding.UTF8.GetBytes(plain);

            byte[] sign;

            using (var hmacSHA256 = new HMACSHA256(secretBytes))
            {
                sign = hmacSHA256.ComputeHash(plainBytes);
            }

            return sign;
        }

        const long MinDiffTime = -1 * 60 * 60 * 1000;
        const long MaxDiffTime = 60 * 60 * 1000;

        /// <summary>
        /// 校验时间戳
        /// </summary>
        /// <param name="timestamp"></param>
        /// <remarks>
        /// 时间误差不能超过一个小时
        /// </remarks>
        /// <returns></returns>
        public static bool ValidateTimestamp(string? timestamp)
        {
            if(!long.TryParse(timestamp, out var millseconds))
            {
                throw new InvalidDataException($"时间戳格式不正确：{timestamp ?? "'null'"}");
            }

            var time = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(millseconds);

            var diff = time - DateTime.UtcNow;

            if(diff.TotalMilliseconds > MaxDiffTime  || diff.Hours < MinDiffTime)
            {
                throw new InvalidDataException("时间戳已过期");
            }

            return true;
        }
    }
}
