using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowershellPreSearch
{
    public class RunPowershellOption
    {
        /// <summary>
        /// 工作目录
        /// </summary>
        public string? WorkingDirectory { get; set; }

        /// <summary>
        /// 脚本文件路径
        /// </summary>
        public List<string> Scripts { get; set; } = new List<string>();
    }
}
