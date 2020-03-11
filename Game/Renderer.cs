using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    static class Renderer
    {
        public static Point MapSection { get; set; }
        public static Point PlayerScreenPosition { get; set; }
        public static int MapSecWidth = 130;
        public static int MapSecHeight = 35;
        public static char[][] Screen;
        public static void Initialize()
        {
            MapSection = new Point(0, 0);
            PlayerScreenPosition = new Point(0, 0);
            Console.SetWindowSize(MapSecWidth + 1, MapSecHeight + 5);
            Console.SetBufferSize(MapSecWidth + 1, MapSecHeight + 5);
            Console.CursorVisible = false;
            Screen = new char[MapSecHeight][];
            for (int i = 0; i < MapSecHeight; i++)
            {
                Screen[i] = new char[MapSecWidth];
            }
        }
        public static void RenderMap()
        {
            Screen = new char[MapSecHeight][];
            for (int i = 0; i < MapSecHeight; i++)
            {
                Screen[i] = new char[MapSecWidth];
                for (int j = 0; j < MapSecWidth; j++)
                {
                    Screen[i][j] = Game.Map[i + MapSection.Y][j + MapSection.X];
                }
            }
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < MapSecHeight; i++)
            {
                for (int j = 0; j < MapSecWidth; j++)
                {
                    Console.Write(Screen[i][j]);
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }
        public static void DrawPlayer()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(PlayerScreenPosition.X, PlayerScreenPosition.Y);
            Console.Write('@');
        }
    }
}
