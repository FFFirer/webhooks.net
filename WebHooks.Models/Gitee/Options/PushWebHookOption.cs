using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Models.Common;

namespace WebHooks.Models.Gitee.Options
{
    public class PushWebHookOption : PlatformOption
    {
        private const string _platform = "Gitee";

        public override string Platform { get => _platform; }

        /// <summary>
        /// 设置在Gitee的密码
        /// </summary>
        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// 针对的分支
        /// </summary>
        public string Branch { get; set; } = "master";

        /// <summary>
        /// 标识勾子的Id
        /// </summary>
        public string HookId { get; set; } = string.Empty;

        /// <summary>
        /// 构建步骤
        /// </summary>
        public List<BuildStep> Steps { get; set; } = new List<BuildStep>();
    }
}
