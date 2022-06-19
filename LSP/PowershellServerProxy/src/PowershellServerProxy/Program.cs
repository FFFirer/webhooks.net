using WebSocketSharp;
using WebSocketSharp.Server;

namespace PowershellServerProxy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var wssv = new WebSocketServer("ws://127.0.0.1:3341");

            wssv.AddWebSocketService<Lsp>("/lsp");
            wssv.Start();
            Console.WriteLine("已启动");   

            Console.ReadKey(true);

            wssv.Stop();
        }
    }

    public class Lsp : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);

            Console.WriteLine($"Received: {e.Data}");
        }
    }
}