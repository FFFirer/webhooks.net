using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Host;
using System.Text;
using System.Threading.Tasks;

namespace Demo01
{
    internal class MyRawUserInterface : PSHostRawUserInterface
    {
        public override ConsoleColor BackgroundColor { get => Console.BackgroundColor; set => Console.BackgroundColor = value; }
        public override Size BufferSize { get => new Size(Console.BufferWidth, Console.BufferHeight); set { } }
        public override Coordinates CursorPosition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override int CursorSize { get => Console.CursorSize; set  {  } }
        public override ConsoleColor ForegroundColor { get => Console.ForegroundColor; set => Console.ForegroundColor = value; }

        public override bool KeyAvailable => Console.KeyAvailable;

        public override Size MaxPhysicalWindowSize => new Size(Console.LargestWindowWidth, Console.LargestWindowHeight);

        public override Size MaxWindowSize => new Size(Console.LargestWindowWidth, Console.LargestWindowHeight);

        public override Coordinates WindowPosition { get => new Coordinates(Console.WindowLeft, Console.WindowTop); set { } }
        public override Size WindowSize { get => new Size(Console.WindowWidth, Console.WindowHeight); set { } }
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
