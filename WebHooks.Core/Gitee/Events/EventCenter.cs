namespace WebHooks.Core.Gitee.Events
{
    public class EventCenter
    {
        private EventCenter()
        {

        }

        public static EventCenter Instance { get; } = new EventCenter();


        private EventHandler<PushEventArgs>? giteePushed;

        /// <summary>
        /// Gitee Push / Tag Push 事件
        /// </summary>
        public event EventHandler<PushEventArgs> GiteePushEvent
        {
            add { giteePushed += value; }
            remove { giteePushed -= value; }
        }

        /// <summary>
        /// Gitee Push / Tag Push 事件触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnGiteePushed(object sender, PushEventArgs args)
        {
            this.giteePushed?.Invoke(sender, args);
        }
    }
}
