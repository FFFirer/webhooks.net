using WebHooks.Core.Gitee;
using WebHooks.Models.Gitee;

namespace WebHooks.Core.Gitee.Events
{
    public class PushEventArgs
    {
        public PushEventArgs(PushWebHook webHook, string @event)
        {
            WebHook = webHook;
            Event = @event;
        }

        public PushWebHook WebHook { get; set; }
        public string Event { get; set; }
    }
}
