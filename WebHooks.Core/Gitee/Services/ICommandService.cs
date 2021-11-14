using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Core.Gitee.Services
{
    public interface ICommandService
    {
        /// <summary>
        /// 根据事件获取要执行的脚本文件路径
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        List<string> GetShellScripts(string @event);

        /// <summary>
        /// 根据事件名称执行脚本
        /// </summary>
        /// <param name="event"></param>
        void RunShellScripts(string @event);
    }
}
