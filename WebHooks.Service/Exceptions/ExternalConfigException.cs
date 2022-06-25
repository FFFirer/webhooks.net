using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Service.Exceptions
{
    /// <summary>
    /// 扩展配置异常
    /// </summary>
    [Serializable]
    public class ExternalConfigException : Exception
    {
        public ExternalConfigException(string message):base(message)
        {

        }

        protected ExternalConfigException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {

        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
