using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Service.Exceptions
{
    [Serializable]
    public class WorkRunningException : Exception
    {
        public WorkRunningException(string message) : base(message)
        {

        }

        protected WorkRunningException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {

        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }
}
