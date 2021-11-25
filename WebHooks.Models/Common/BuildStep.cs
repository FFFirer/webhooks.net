using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Models.Common
{
    /// <summary>
    /// 构建步骤
    /// </summary>
    public class BuildStep
    {
        /// <summary>
        /// 环境变量，暂时不启用
        /// </summary>
        public Dictionary<string, string> EnvVariables { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 执行的脚本命令
        /// </summary>
        public List<string> Scripts { get; set; } = new List<string>();
    }
}
