namespace WebHooks.Gitee.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// 字节数组转换为十六进制字符串
        /// </summary>
        /// <param name="bytes">待转的字节数组</param>
        /// <param name="seperator">16进制字符串分隔符</param>
        /// <returns>
        /// 16进制组成的字符串
        /// </returns>
        public static string ToHexString(this byte[] bytes, string seperator = "")
        {
            var hexStrings = bytes.Select(b => Convert.ToString(b, 16)).ToArray();

            return string.Join(seperator, hexStrings);
        }
    }
}
