using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gitee.WebHooks.Models.Base
{
    public class Project
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// 仓库名。eg：gitee
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 仓库所属的空间地址。eg：oschian
        /// </summary>
        [JsonPropertyName("path")]
        public string? Path { get; set; }

        /// <summary>
        /// 完整的名字，name + path。eg：gitee/oschian
        /// </summary>
        [JsonPropertyName("full_name")]
        public string? FullName { get; set; }

        /// <summary>
        /// 仓库的所有者。
        /// </summary>
        [JsonPropertyName("owner")]
        public User? Owner { get; set; }

        /// <summary>
        /// 是否公开。
        /// </summary>
        [JsonPropertyName("private")]
        public bool Private { get; set; }

        /// <summary>
        /// 对应 Gitee 的 url。eg：https://gitee.com/oschina/git-osc
        /// </summary>
        [JsonPropertyName("html_url")]
        public string? HtmlUrl { get; set; }

        /// <summary>
        /// 与上面 <see cref="Project.Url"/> 一致
        /// </summary>
        [JsonPropertyName("url")]
        public string? Url { get; set; }

        /// <summary>
        /// 仓库描述。eg：这是一个开源仓库...
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// 是不是 fork 仓库。
        /// </summary>
        [JsonPropertyName("fork")]
        public bool Fork { get; set; }

        /// <summary>
        /// 仓库的创建时间。eg：2020-01-01T00:00:00+08:00
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 仓库的更新时间。eg：2020-01-01T00:00:00+08:00
        /// </summary>
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 仓库的最近一次推送时间。eg：2020-01-01T00:00:00+08:00
        /// </summary>
        [JsonPropertyName("pushed_at")]
        public DateTime PushedAt { get; set; }

        /// <summary>
        /// 仓库的 git 地址。eg：git://gitee.com:oschina/git-osc.git
        /// </summary>
        [JsonPropertyName("git_url")]
        public string? GitUrl { get; set; }

        /// <summary>
        /// 仓库的 ssh 地址。eg：git@gitee.com:oschina/git-osc.git
        /// </summary>
        [JsonPropertyName("ssh_url")]
        public string? SshUrl { get; set; }

        /// <summary>
        /// 仓库的 clone 地址。eg：https://gitee.com/oschina/git-osc.git
        /// </summary>
        [JsonPropertyName("clone_url")]
        public string? CloneUrl { get; set; }

        /// <summary>
        /// 仓库的 svn 地址。eg：svn://gitee.com/oschina/git-osc
        /// </summary>
        [JsonPropertyName("svn_url")]
        public string? SvnUrl { get; set; }

        /// <summary>
        /// 与上面的 <see cref="Project.CloneUrl"/> 一致。
        /// </summary>
        [JsonPropertyName("git_http_url")]
        public string? GitHttpUrl { get; set; }

        /// <summary>
        /// 与上面的 <see cref="Project.SshUrl"/> 一致。
        /// </summary>
        [JsonPropertyName("git_ssh_url")]
        public string? GitSshUrl { get; set; }

        /// <summary>
        /// 与上面的 <see cref="Project.SvnUrl"/> 一致。
        /// </summary>
        [JsonPropertyName("git_svn_url")]
        public string? GitSvnUrl { get; set; }

        /// <summary>
        /// 仓库的网页主页。eg：https://gitee.com
        /// </summary>
        [JsonPropertyName("homepage")]
        public object? HomePage { get; set; }

        /// <summary>
        /// 仓库的 star 数量。
        /// </summary>
        [JsonPropertyName("stargazers_count")]
        public int StargazersCount { get; set; }

        /// <summary>
        /// 仓库的 watch 数量。
        /// </summary>
        [JsonPropertyName("watchers_count")]
        public int WatchersCount { get; set; }

        /// <summary>
        /// 仓库的 fork 数量。
        /// </summary>
        [JsonPropertyName("forks_count")]
        public int ForksCount { get; set; }

        /// <summary>
        /// 仓库的编程语言。eg： Ruby
        /// </summary>
        [JsonPropertyName("language")]
        public string? Language { get; set; }

        /// <summary>
        /// 仓库的是否开启了 issue 功能。
        /// </summary>
        [JsonPropertyName("has_issues")]
        public bool HasIssues { get; set; }

        /// <summary>
        /// 仓库的是否开启了 wiki 功能。
        /// </summary>
        [JsonPropertyName("has_wiki")]
        public bool HasWiki { get; set; }

        /// <summary>
        /// 仓库的是否开启了 page 服务。
        /// </summary>
        [JsonPropertyName("has_pages")]
        public bool HasPages { get; set; }

        /// <summary>
        /// 仓库的开源协议。eg：MIT
        /// </summary>
        [JsonPropertyName("license")]
        public object? License { get; set; }

        /// <summary>
        /// 仓库开启状态的 issue 总数。
        /// </summary>
        [JsonPropertyName("open_issues_count")]
        public int OpenIssuesCount { get; set; }

        /// <summary>
        /// 仓库的默认复制。eg：master
        /// </summary>
        [JsonPropertyName("default_branch")]
        public string? DefaultBranch { get; set; }

        /// <summary>
        /// 仓库所属的 Gitee 地址。eg：oschina
        /// </summary>
        [JsonPropertyName("namespace")]
        public string? Namespace { get; set; }

        /// <summary>
        /// 与上面的 <see cref="Project.FullName"/> 一致。
        /// </summary>
        [JsonPropertyName("name_with_namespace")]
        public string? NameWithNamespace { get; set; }

        /// <summary>
        /// 仓库的在 Gitee 的资源唯一标识。eg：oschia/git-osc
        /// </summary>
        [JsonPropertyName("path_with_namespace")]
        public string? PathWithNamespace { get; set; }
    }
}
