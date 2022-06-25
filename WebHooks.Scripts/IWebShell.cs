using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

namespace WebHooks.Scripts
{
    public interface IWebShell : IDisposable
    {
        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="scripts"></param>
        /// <returns></returns>
        Task<(int, PSDataCollection<PSObject>)> ExecuteAsync(string script);

        Task<(int, IEnumerable<string>)> ExecuteScriptAsync(string script);
    }
}
