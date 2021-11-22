using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Models.Gitee.Options;

namespace WebHooks.Core.Commands
{
    /// <summary>
    /// 构建一个Powershell命令行的客户端实例
    /// </summary>
    public class PowershellClient : IDisposable
    {
        private List<DataReceivedEventHandler> receivedHandlers = new List<DataReceivedEventHandler>();

        public void AddHandler(DataReceivedEventHandler handler)
        {
            receivedHandlers.Add(handler);
        }

        private Process? process { get; set; }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command"></param>
        public void Execute(string command)
        {

        }

        /// <summary>
        /// 添加临时环境变量
        /// </summary>
        /// <param name="variables">要添加的环境变量</param>
        public void AddEnvironmentVariables(Dictionary<string, string> variables)
        {

        }

        public void Dispose()
        {
            if(process != null)
            {
                process.Dispose();
            }
        }
    }
}
