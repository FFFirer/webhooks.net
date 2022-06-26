using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gitee.WebHooks.Models.Base
{
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// 用户的昵称。eg：红薯
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 用户的邮箱。eg：git@oschina.cn
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>
        /// 用户的 Gitee 个人空间地址。eg：gitee
        /// </summary>
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        /// <summary>
        /// 与上面的 <see cref="User.Username"/> 一致。
        /// </summary>
        [JsonPropertyName("user_name")]
        public string? UserName { get; set; }

        /// <summary>
        /// 用户的 Gitee 个人主页 url。eg：https://gitee.com/gitee
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        /// <summary>
        /// 与上面的 <see cref="User.Username"/> 一致。
        /// </summary>
        [JsonPropertyName("login")]
        public string? Login { get; set; }

        /// <summary>
        /// 用户头像 url。eg：https://gitee.com/assets/favicon.ico
        /// </summary>
        [JsonPropertyName("avatar_url")]
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// 与上面的 <see cref="User.Url"/> 一致。
        /// </summary>
        [JsonPropertyName("html_url")]
        public string? HtmlUrl { get; set; }

        /// <summary>
        /// 用户类型，目前固定为 User。
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// 是不是管理员。
        /// </summary>
        [JsonPropertyName("site_admin")]
        public bool SiteAdmin { get; set; }

        /// <summary>
        /// git commit 中的时间。eg：2020-01-01T00:00:00+08:00
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime? Time { get; set; }

        /// <summary>
        /// 用户备注名。eg：Ruby 大神
        /// </summary>
        [JsonPropertyName("remark")]
        public string? Remark { get; set; }
    }
}
