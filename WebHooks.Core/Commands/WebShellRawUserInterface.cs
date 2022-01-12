using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Host;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Core.Commands
{
    public class WebShellRawUserInterface : PSHostRawUserInterface
    {
        private ConsoleColor _backgroundColor => ConsoleColor.White;
        private ConsoleColor _foregroundColor => ConsoleColor.Red;
        private int _bufferWidth => 100;
        private int _bufferHeight => 3000;
        private int _cursorSize => 20;
        private bool _keyAvailable => false;
        private int _largestWindowWidth => int.MaxValue;
        private int _largestWindowHeight => int.MaxValue;
        private int _windowLeft => 0;
        private int _windowTop => 0;
        private int _windowWidth = 100;
        private int _windowHeight = 100;

        public override ConsoleColor BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {

            }
        }
        public override Size BufferSize
        {
            get
            {
                return new Size(_bufferWidth, _bufferHeight);
            }
            set
            {

            }
        }
        public override Coordinates CursorPosition
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        public override int CursorSize
        {
            get
            {
                return _cursorSize;
            }
            set
            {

            }
        }
        public override ConsoleColor ForegroundColor
        {
            get
            {
                return _foregroundColor;
            }
            set
            {

            }
        }

        public override bool KeyAvailable => _keyAvailable;

        public override Size MaxPhysicalWindowSize => throw new NotImplementedException();

        public override Size MaxWindowSize => new Size(_largestWindowWidth, _largestWindowHeight);

        public override Coordinates WindowPosition { get => new Coordinates(_windowLeft, _windowTop); set { } }
        public override Size WindowSize { get => new Size(_windowWidth, _windowHeight); set { } }
        public override string WindowTitle { get; set; } = String.Empty;

        public override void FlushInputBuffer()
        {
            
        }

        public override BufferCell[,] GetBufferContents(Rectangle rectangle)
        {
            throw new NotImplementedException();
        }

        public override KeyInfo ReadKey(ReadKeyOptions options)
        {
            throw new NotImplementedException();
        }

        public override void ScrollBufferContents(Rectangle source, Coordinates destination, Rectangle clip, BufferCell fill)
        {
            throw new NotImplementedException();
        }

        public override void SetBufferContents(Coordinates origin, BufferCell[,] contents)
        {
            throw new NotImplementedException();
        }

        public override void SetBufferContents(Rectangle rectangle, BufferCell fill)
        {
            throw new NotImplementedException();
        }
    }
}
