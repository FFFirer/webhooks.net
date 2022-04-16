using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.PowerShell
{
    public sealed class ProcessSettings
    {
        /// <summary>
        /// 工作目录
        /// </summary>
        public string? WorkingDirectory { get; set; }

        /// <summary>
        /// 重定向标准错误
        /// </summary>
        public bool RedirectStandardError { get; set; } = false;

        /// <summary>
        /// 重定向标准输出
        /// </summary>
        public bool RedirectStandardOutput { get; set; } = false;

        /// <summary>
        /// 等待退出超时时间
        /// </summary>
        public int? Timeout { get; set; }

        /// <summary>
        /// 是否抑制输出
        /// </summary>
        public bool Slient { get; set; } = true;

        /// <summary>
        /// 环境变量
        /// </summary>
        public IDictionary<string, string> EnvironmentVariables { get; set; } = new Dictionary<string, string>();
    }
}
