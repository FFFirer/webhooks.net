using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.PowerShell.Helpers
{
    public static class ProcessHelper
    {
        public static void SetEnvironmentVariable(ProcessStartInfo info, string key, string value)
        {
            var environmentKey = info.Environment.Keys.FirstOrDefault(existingKey => existingKey.Equals(key, StringComparison.OrdinalIgnoreCase)) ?? key;
            info.Environment[environmentKey] = value;
        }
    }
}
