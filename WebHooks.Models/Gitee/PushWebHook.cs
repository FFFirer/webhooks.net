using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using WebHooks.Models.Gitee;
using System.ComponentModel;

namespace WebHooks.Models.Gitee
{
    public class PushWebHook
    {
        /// <summary>
        /// 钩子 id。
        /// </summary>
        [JsonPropertyName("hook_id")]
        public int HookId { get; set; }

        /// <summary>
        /// 钩子名，固定为 push_hooks/tag_push_hooks。
        /// </summary>
        [JsonPropertyName("hook_name")]
        public string? HookName { get; set; }

        /// <summary>
        /// 钩子密码。eg：123456
        /// </summary>
        [JsonPropertyName("password")]
        public string? Password { get; set; }

        /// <summary>
        /// 钩子路由。
        /// </summary>
        [JsonPropertyName("hook_url")]
        public string? HookUrl { get; set; }

        /// <summary>
        /// 触发钩子的时间戳。eg: 1576754827988
        /// </summary>
        [JsonPropertyName("timestamp")]
        public string? Timestamp { get; set; }

        /// <summary>
        /// 钩子根据密钥计算的签名。eg: "rLEHLuZRIQHuTPeXMib9Czoq9dVXO4TsQcmQQHtjXHA="
        /// </summary>
        [JsonPropertyName("sign")]
        public string? Sign { get; set; }

        /// <summary>
        /// 推送的分支。eg：refs/heads/master
        /// </summary>
        [JsonPropertyName("ref")]
        public string? Ref { get; set; }

        /// <summary>
        /// 推送前分支的 commit id。eg：5221c062df39e9e477ab015df22890b7bf13fbbd
        /// </summary>
        [JsonPropertyName("before")]
        public string? Before { get; set; }

        /// <summary>
        /// 推送后分支的 commit id。eg：1cdcd819599cbb4099289dbbec762452f006cb40
        /// </summary>
        [JsonPropertyName("after")]
        public string? After { get; set; }

        /// <summary>
        /// 推送的是否是新分支。
        /// </summary>
        [JsonPropertyName("created")]
        public bool Created { get; set; }

        /// <summary>
        /// 推送的是否是删除分支。
        /// </summary>
        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }

        /// <summary>
        /// 推送的 commit 差异 url。eg：https://gitee.com/oschina/git-osc/compare/5221c062df39e9e477ab015df22890b7bf13fbbd...1cdcd819599cbb4099289dbbec762452f006cb40
        /// </summary>
        [JsonPropertyName("compare")]
        public string? Compare { get; set; }

        /// <summary>
        /// 推送的全部 commit 信息。
        /// </summary>
        [JsonPropertyName("commits")]
        public Commit[]? Commits { get; set; }

        /// <summary>
        /// 推送最前面的 commit 信息。
        /// </summary>
        [JsonPropertyName("head_commit")]
        public Commit? HeadCommit { get; set; }

        /// <summary>
        /// 推送包含的 commit 总数。
        /// </summary>
        [JsonPropertyName("total_commits_count")]
        public int? TotalCommitsCount { get; set; }

        /// <summary>
        /// 推送包含的 commit 总数是否大于十二。
        /// </summary>
        [JsonPropertyName("commits_more_than_ten")]
        public bool? CommitsMoreThanTen { get; set; }

        /// <summary>
        /// 推送的目标仓库信息。
        /// </summary>
        [JsonPropertyName("repository")]
        public Project? Repository { get; set; }

        /// <summary>
        /// 推送的目标仓库信息。
        /// </summary>
        [JsonPropertyName("project")]
        public Project? Project { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// 推送者的昵称。
        /// </summary>
        [JsonPropertyName("user_name")]
        public string? UserName { get; set; }

        /// <summary>
        /// 推送者的用户信息。
        /// </summary>
        [JsonPropertyName("user")]
        public User? User { get; set; }

        /// <summary>
        /// 推送者的用户信息。
        /// </summary>
        [JsonPropertyName("pusher")]
        public User? Pusher { get; set; }

        /// <summary>
        /// 推送者的用户信息。
        /// </summary>
        [JsonPropertyName("sender")]
        public User? Sender { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("enterprise")]
        public Enterprise? Enterprise { get; set; }
    }
}
