using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Service.WorkRunner.External
{
    public class BaseExternalWorkRunner
    {
        protected private ILogger _logger;

        public BaseExternalWorkRunner(ILogger logger)
        {
            this._logger = logger;
        }
    }
}
