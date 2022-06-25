using LibGit2Sharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Entities;
using WebHooks.Data.Repositories.Interfaces;
using WebHooks.Scripts;
using WebHooks.Service.Dtos;
using WebHooks.Service.Exceptions;
using WebHooks.Service.Interfaces;
using Version = System.Version;

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
        private readonly IWebShellFactory _webshells;
        private readonly IWorkExecutionLogService _executionLogs;

        private WorkExecutionLog? executionLog { get; set; }

        public WorkRunner(IServiceProvider serviceProvider,
            IOptionsMonitor<ExternalWorkRunnerBuildOption> options,
            ISettingService settingService,
            IBuildScriptService scriptService,
            IWorkRepository workService,
            IWebShellFactory webShellFactory,
            IWorkExecutionLogService executionLogService)
        {
            this._serviceProvider = serviceProvider;
            this._options = options;
            this._settings = settingService;
            this._scripts = scriptService;
            this._works = workService;
            this._webshells = webShellFactory;
            this._executionLogs = executionLogService;
        }

        public async Task RunAsync(Work work)
        {
            this.executionLog = await _executionLogs.Create(work.Id);

            try
            {
                this.executionLog.Status = Data.Constants.WebExecutionStatus.Executing;   // 执行中
                this.executionLog.ExecuteStartAt = DateTime.UtcNow;   // 开始时间
                await _executionLogs.Update(this.executionLog);

                // 内部执行
                await InternalRun(work);

                // 扩展执行
                var externalWorkRunningContext = new WorkRunningContext(work);
                var externalWorkRunner = GetExternalWorkRunner(work);
                await externalWorkRunner.Run(externalWorkRunningContext);

                // 执行脚本
                var results = await RunScripts(work);

                this.executionLog.Results = results;  // 结果
                this.executionLog.Success = true; // 成功
            }
            catch (Exception ex)
            {
                this.executionLog.Exception = ex.ToString();  // 异常信息
                this.executionLog.Success = false;    // 失败
            }
            finally
            {
                this.executionLog.ExecuteEndAt = DateTime.UtcNow; // 结束时间
                this.executionLog.Status = Data.Constants.WebExecutionStatus.Completed;  // 状态完成
                this.executionLog.ElapsedTime = this.executionLog.ExecuteEndAt - executionLog.ExecuteStartAt;
                await _executionLogs.Update(this.executionLog);
            }
        }

        /// <summary>
        /// 内部运行
        /// </summary>
        /// <param name="work"></param>
        protected virtual async Task InternalRun(Work work)
        {
            var basicSetting = await _settings.GetBasicSettingAsync();

            if (string.IsNullOrEmpty(basicSetting.BaseWorkDirectory))
            {
                throw new WorkRunningException("未指定基础工作目录");
            }

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
        protected virtual async Task<List<ShellExecutedResultLine>> RunScripts(Work work)
        {
            var script = await _scripts.GetAsync(work.Id);


            if (script != null)
            {
                if (this.executionLog == null)
                {
                    throw new WorkRunningException($"未记录工作项[{work.Id}]执行记录");
                }

                this.executionLog.Script = script.Script;
                await _executionLogs.Update(this.executionLog);

                // 执行脚本
                var starupInfo = new WebShellStartupInfo(work.DisplayName ?? "default", work.WorkingDirectory!, new Version(1, 0, 0), WebShellTypes.PowerShell);
                using (var shell = _webshells.Create(starupInfo))
                {
                    var (exitCode, results) = await shell.ExecuteAsync(script.ToString()!);

                    return GetFriendlyResult(results).ToList();
                }
            }

            return new List<ShellExecutedResultLine>();
        }

        protected virtual IEnumerable<ShellExecutedResultLine> GetFriendlyResult(PSDataCollection<PSObject> results)
        {
            foreach (var line in results.ReadAll())
            {
                if (line.BaseObject is ErrorRecord errorRecord)
                {
                    var errorResult = new ShellExecutedResultLine(Data.Constants.ResultLineLevel.Error, errorRecord.ToString())
                    {
                        Exception = errorRecord.Exception.ToString(),
                        StackTrace = errorRecord.ScriptStackTrace.ToString()
                    };
                    yield return errorResult;
                }
                else
                {
                    var result = new ShellExecutedResultLine(Data.Constants.ResultLineLevel.Info, line.ToString());
                    yield return result;
                }
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
