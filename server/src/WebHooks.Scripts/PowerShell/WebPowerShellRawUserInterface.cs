using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation.Host;

namespace WebHooks.Scripts.PowerShell
{
    public class WebPowerShellRawUserInterface : PSHostRawUserInterface
    {
        private readonly BufferCell[,] _buffer;

        public WebPowerShellRawUserInterface()
        {
            this._buffer = new BufferCell[0, 0];
        }

        public override ConsoleColor BackgroundColor { get; set; }
        public override Size BufferSize { get; set; }
        public override Coordinates CursorPosition { get; set; }
        public override int CursorSize { get; set; }
        public override ConsoleColor ForegroundColor { get; set; }

        public override bool KeyAvailable => false;

        public override Size MaxPhysicalWindowSize => MaxWindowSize;

        public override Size MaxWindowSize => new() { Width = _buffer.GetLength(0), Height = _buffer.GetLength(1) };

        public override Coordinates WindowPosition { get; set; }
        public override Size WindowSize { get; set; }
        public override string WindowTitle { get; set; } = default!;

        public override void FlushInputBuffer()
        {
           
        }

        public override BufferCell[,] GetBufferContents(Rectangle rectangle) => _buffer;

        public override KeyInfo ReadKey(ReadKeyOptions options) => default;

        public override void ScrollBufferContents(Rectangle source, Coordinates destination, Rectangle clip, BufferCell fill)
        {
           
        }

        public override void SetBufferContents(Coordinates origin, BufferCell[,] contents)
        {
            
        }

        public override void SetBufferContents(Rectangle rectangle, BufferCell fill)
        {
            
        }
    }
}
