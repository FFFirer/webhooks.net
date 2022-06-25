using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;
using WebHooks.Data.Repositories.Interfaces;
using WebHooks.Service.Dtos;
using WebHooks.Service.Exceptions;
using WebHooks.Service.Interfaces;

namespace WebHooks.Service.WorkRunner
{
    /// <summary>
    /// 工作执行器
    /// </summary>
    public class WorkRunner : IWorkRunner
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptionsMonitor<ExternalWorkRunnerBuildOption> _options;
        private readonly ISettingService _settings;
        private readonly IBuildScriptService _scripts;
        private readonly IWorkRepository _works;

        public WorkRunner(IServiceProvider serviceProvider,
            IOptionsMonitor<ExternalWorkRunnerBuildOption> options,
            ISettingService settingService,
            IBuildScriptService scriptService,
            IWorkRepository workService)
        {
            this._serviceProvider = serviceProvider;
            this._options = options;
            this._settings = settingService;
            this._scripts = scriptService;
            this._works = workService;
        }

        public async Task RunAsync(Work work)
        {
            // 内部执行
            await InternalRun(work);

            // 扩展执行
            var externalWorkRunningContext = new WorkRunningContext(work);
            var externalWorkRunner = GetExternalWorkRunner(work);
            await externalWorkRunner.Run(externalWorkRunningContext);

            // 执行脚本
            await RunScripts(work);
        }

        /// <summary>
        /// 内部运行
        /// </summary>
        /// <param name="work"></param>
        protected virtual async Task InternalRun(Work work)
        {
            var basicSetting = await _settings.GetBasicSettingAsync();
           
            if (!Path.IsPathRooted(basicSetting.BaseWorkDirectory))
            {
                basicSetting.BaseWorkDirectory = Path.Combine(Environment.CurrentDirectory, basicSetting.BaseWorkDirectory);
            }
            
            if (string.IsNullOrEmpty(work.WorkingDirectory))
            {
                work.WorkingDirectory = GetWorkWorkDirectory(basicSetting, work);
            }
            else if (!work.WorkingDirectory.StartsWith(basicSetting.BaseWorkDirectory))
            {
                EnsureRemoveWorkingDirectory(work);
                work.WorkingDirectory = GetWorkWorkDirectory(basicSetting, work);
                await _works.UpdateAsync(work);
            }

            // 判断路径是否存在并创建
            EnsureWorkingDirectory(work.WorkingDirectory);
        }

        protected virtual string GetWorkWorkDirectory(BasicSetting basicSetting, Work work)
        {
            if (string.IsNullOrEmpty(basicSetting.BaseWorkDirectory))
            {
                throw new WorkRunningException("未设置基本工作目录");
            }

            return Path.Combine(basicSetting.BaseWorkDirectory, work.Id.ToString());
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="work"></param>
        /// <returns></returns>
        protected virtual async Task RunScripts(Work work)
        {
            var script = await _scripts.GetAsync(work.Id);

            if (script != null)
            {
                // 执行脚本
            }
        }

        /// <summary>
        /// 确保工作目录存在
        /// </summary>
        /// <param name="path"></param>
        protected virtual void EnsureWorkingDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 确保移除工作目录
        /// </summary>
        /// <param name="path"></param>
        protected virtual void EnsureRemoveWorkingDirectory(Work work)
        {
            if (Directory.Exists(work.WorkingDirectory))
            {
                Directory.Delete(work.WorkingDirectory, true);
            }
        }

        /// <summary>
        /// 工作目录变动确保迁移
        /// </summary>
        /// <param name="old"></param>
        /// <param name="current"></param>
        protected virtual void EnsureMoveWorkingDirectory(string old, string current)
        {

        }

        /// <summary>
        /// 获取扩展配置执行器
        /// </summary>
        /// <param name="work"></param>
        /// <returns></returns>
        /// <exception cref="WorkRunningException"></exception>
        protected virtual IExternalWorkRunner GetExternalWorkRunner(Work work)
        {
            var externalWorkRunnerOption = _options.Get(work.ExternalConfigType);

            if (externalWorkRunnerOption == null)
            {
                throw new WorkRunningException($"获取外部工作项({(work.ExternalConfigType ?? "")})配置失败");
            }

            if (externalWorkRunnerOption.BuildFrom == null)
            {
                throw new WorkRunningException($"扩展配置运行器构造方法为空");
            }

            IExternalWorkRunner externalWorkRunner = externalWorkRunnerOption.BuildFrom(_serviceProvider);

            return externalWorkRunner;
        }
    }
}
