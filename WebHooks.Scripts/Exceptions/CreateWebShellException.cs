using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Scripts.Exceptions
{
    [Serializable]
    public class CreateWebShellException : Exception
    {
        public CreateWebShellException(string message) : base(message) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        protected CreateWebShellException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
            
        }
    }
}
