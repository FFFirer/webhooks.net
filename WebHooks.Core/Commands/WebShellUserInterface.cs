using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Core.Commands
{
    public class WebShellUserInterface : PSHostUserInterface
    {
        public override PSHostRawUserInterface RawUI => new WebShellRawUserInterface();

        public event EventHandler<string>? OutputEventHandker;

        /// <summary>
        /// 输出事件
        /// </summary>
        /// <param name="message"></param>
        private void OnOutput(string message)
        {
            OutputEventHandker?.Invoke(this, message);
        }

        public override Dictionary<string, PSObject> Prompt(string caption, string message, Collection<FieldDescription> descriptions)
        {
            throw new NotImplementedException();
        }

        public override int PromptForChoice(string caption, string message, Collection<ChoiceDescription> choices, int defaultChoice)
        {
            throw new NotImplementedException();
        }

        public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName)
        {
            throw new NotImplementedException();
        }

        public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName, PSCredentialTypes allowedCredentialTypes, PSCredentialUIOptions options)
        {
            throw new NotImplementedException();
        }

        public override string ReadLine()
        {
            return string.Empty;
        }

        public override SecureString ReadLineAsSecureString()
        {
            throw new NotImplementedException();
        }

        public override void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
        {
            // 输出
            OnOutput(value);
        }

        public override void Write(string value)
        {
            // 输出
            OnOutput(value);
        }

        public override void WriteDebugLine(string message)
        {
            // 输出
            OnOutput($"[DEBUG] {message}\n");
        }

        public override void WriteErrorLine(string value)
        {
            // 输出
            OnOutput($"[ERROR] {value}\n");
        }

        public override void WriteLine(string value)
        {
            // 输出
            OnOutput($"{value}\n");
        }

        public override void WriteProgress(long sourceId, ProgressRecord record)
        {
            OnOutput($"[Process] {sourceId} {record.CurrentOperation} {record.StatusDescription}");
        }

        public override void WriteVerboseLine(string message)
        {
            // 输出
            OnOutput($"[VERBOSE] {message}\n");
        }

        public override void WriteWarningLine(string message)
        {
            // 输出
            OnOutput($"[WARNING] {message}\n");
        }
    }
}
