using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Shared.CustomExceptions
{
    /// <summary>
    /// 数据校验异常
    /// </summary>
    /// <remarks>
    /// 输入数据校验结果不符合要求
    /// </remarks>
    public class DataValidationException: Exception
    {
        public DataValidationException(string message) : base(message)
        {

        }
    }
}
