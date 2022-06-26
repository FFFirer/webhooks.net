using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Constants;

namespace WebHooks.Data.Entities
{
    public class ShellExecutedResultLine
    {
        public ShellExecutedResultLine(ResultLineLevel level, string message)
        {
            this.Level = level;
            this.Message = message;
        }

        public ResultLineLevel Level { get; set; }
        public string Message { get; set; }
        public string? StackTrace { get; set; }
        public string? Exception { get; set; }
    }
}
