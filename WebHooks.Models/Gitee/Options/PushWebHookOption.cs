using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Models.Gitee.Options
{
    public class PushWebHookOption
    {
        public const string Platform = "Gitee";

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string Repository { get; set; } = string.Empty;

        /// <summary>
        /// 密钥，和配置在Gitee端的密钥保持一致
        /// </summary>
        public string Secret { get; set; } = string.Empty ;

        /// <summary>
        /// 使用的命令行程序
        /// </summary>
        public string Application { get; set; } = string.Empty ;

        /// <summary>
        /// 要执行的脚本
        /// </summary>
        public List<string> Scripts { get; set; } = new List<string>();

        /// <summary>
        /// 环境变量
        /// </summary>
        public Dictionary<string, string> EnvironmentVariables { get; set; } = new Dictionary<string, string>();
    }
}
