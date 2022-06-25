using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Scripts
{
    public class WebShellStartupInfo
    {
        public string WebShellType { get; set; }
        public string Name { get; set; }
        public string WorkingDirectory { get; set; }
        public Version Version { get; set; }

        public WebShellStartupInfo(string name, string workingDirectory, Version version, string webShellType)
        {
            Name = name;
            WorkingDirectory = workingDirectory;
            Version = version;
            WebShellType = webShellType;
        }
    }
}
