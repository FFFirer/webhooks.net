using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.EntityFrameworkCore.Pgsql.Exceptions
{
    /// <summary>
    /// 文件未找到错误
    /// </summary>
    [Serializable]
    public class FileNotFoundException : Exception
    {
        public FileNotFoundException(string message) : base(message) { }

        protected FileNotFoundException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext):base(serializationInfo, streamingContext)
        {
            
        }
    }
}
