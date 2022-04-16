using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.PowerShell.Helpers;

namespace WebHooks.PowerShell
{
    public class ProcessRunner : IProcessRunner
    {
        public ProcessRunner()
        {

        }

        public IProcess Start(string? filePath, ProcessSettings? settings)
        {
            // 校验文件路径
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }


            if(settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var info = GetProcessStartInfo(filePath, settings);

            var process = new Process()
            {
                StartInfo = info,
                EnableRaisingEvents = true,
            };

            // 退出事件注册
            process.Start();

            var wrapper = new ProcessWrapper(process);

            if (settings.RedirectStandardError)
            {
                SubscribeStandardError(process, wrapper);
            }

            if (settings.RedirectStandardOutput)
            {
                SubscribeStandardOutput(process, wrapper);
            }

            return wrapper;
        }

        internal ProcessStartInfo GetProcessStartInfo(string filePath, ProcessSettings settings)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(nameof(filePath));
            }

            var fileName = Path.GetFileName(filePath);

            var info = new ProcessStartInfo(fileName)
            {
                UseShellExecute = false,
                RedirectStandardError = settings.RedirectStandardError,
                RedirectStandardOutput = settings.RedirectStandardOutput,
            };

            if (!string.IsNullOrEmpty(settings.WorkingDirectory))
            {
                // 设置工作目录
                info.WorkingDirectory = settings.WorkingDirectory;  // 完整的绝对路径
            }

            info.Arguments = "Get-ChildItem";

            if(settings.EnvironmentVariables.Keys.Any())
            {
                foreach (var environment in settings.EnvironmentVariables)
                {
                    ProcessHelper.SetEnvironmentVariable(info, environment.Key, environment.Value);
                }
            }

            return info;
        }
    
        private static void SubscribeStandardError(Process process, ProcessWrapper processWrapper)
        {
            process.ErrorDataReceived += (sender, e) =>
            {
                processWrapper.StandardErrorReceived(e.Data);
            };
            process.BeginErrorReadLine();
        }

        private static void SubscribeStandardOutput(Process process, ProcessWrapper processWrapper)
        {
            process.OutputDataReceived += (sender, e) =>
            {
                processWrapper.StandardOutputReceived(e.Data);
            };
            process.BeginOutputReadLine();
        }
    }
}
