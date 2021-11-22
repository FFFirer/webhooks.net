using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Models.Gitee.Options;

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

        /// <summary>
        /// 跑关于Push钩子的命令行
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        Task RunScripts(PushWebHookOption option);
    }
}
