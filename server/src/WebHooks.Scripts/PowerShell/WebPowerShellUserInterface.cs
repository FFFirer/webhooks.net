using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation.Host;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Security;

namespace WebHooks.Scripts.PowerShell
{
    public class WebPowerShellUserInterface : PSHostUserInterface
    {
        public override PSHostRawUserInterface RawUI => new WebPowerShellRawUserInterface();

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
            return String.Empty;
        }

        public override SecureString ReadLineAsSecureString()
        {
            throw new NotImplementedException();
        }

        public override void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
        {
            
        }

        public override void Write(string value)
        {
            
        }

        public override void WriteDebugLine(string message)
        {
            
        }

        public override void WriteErrorLine(string value)
        {
            
        }

        public override void WriteLine(string value)
        {
            
        }

        public override void WriteProgress(long sourceId, ProgressRecord record)
        {
            
        }

        public override void WriteVerboseLine(string message)
        {
            
        }

        public override void WriteWarningLine(string message)
        {
            
        }
    }
}
