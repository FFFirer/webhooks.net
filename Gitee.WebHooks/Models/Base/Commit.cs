using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gitee.WebHooks.Models.Base
{
    public class Commit
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// commit tree oid。eg：db78f3594ec0683f5d857ef731df0d860f14f2b2
        /// </summary>
        [JsonPropertyName("tree_id")]
        public string? TreeId { get; set; }

        /// <summary>
        /// commit parent_ids。eg：['a3bddf21a35af54348aae5b0f5627e6ba35be51c']
        /// </summary>
        [JsonPropertyName("parent_ids")]
        public List<string>? ParentIds { get; set; }

        /// <summary>
        /// commit 的信息。eg：fix(cache): 修复了缓存问题
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        /// <summary>
        /// commit 的时间。eg：2020-01-01T00:00:00+08:00
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// commit 对应的 Gitee url。eg：https://gitee.com/mayun-team/oauth2_dingtalk/commit/664b34859fc4a924cd60be2592c0fc788fbeaf8f
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        /// <summary>
        /// 作者信息。
        /// </summary>
        [JsonPropertyName("author")]
        public User? Author { get; set; }

        /// <summary>
        /// 提交者信息。
        /// </summary>
        [JsonPropertyName("committer")]
        public User? Committer { get; set; }

        /// <summary>
        /// 特殊的 commit，没任何改动，如 tag
        /// </summary>
        [JsonPropertyName("distinct")]
        public bool Distinct { get; set; }

        /// <summary>
        /// 新加入的文件名。eg：['README.md']
        /// </summary>
        [JsonPropertyName("added")]
        public List<string>? Added { get; set; }

        /// <summary>
        /// 被移除的文件名。eg：['README.md']
        /// </summary>
        [JsonPropertyName("removed")]
        public List<string>? Removed { get; set; }

        /// <summary>
        /// 修改过的文件名。eg：['README.md']
        /// </summary>
        [JsonPropertyName("modified")]
        public List<string>? Modified { get; set; }
    }
}
