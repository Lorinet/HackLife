using System;
using System.Text;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            while(true) MainMenu();
        }
        static void MainMenu()
        {
            Console.Clear();
            Console.SetWindowSize(66, 20);
            Console.SetBufferSize(66, 20);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(" _______ _______ _______ _______ _______ _______ _______ _______ \n|\\     /|\\     /|\\     /|\\     /|\\     /|\\     /|\\     /|\\     /|\n| +---+ | +---+ | +---+ | +---+ | +---+ | +---+ | +---+ | +---+ |\n| |H  | | |a  | | |c  | | |k  | | |L  | | |i  | | |f  | | |e  | |\n| +---+ | +---+ | +---+ | +---+ | +---+ | +---+ | +---+ | +---+ |\n|/_____\\|/_____\\|/_____\\|/_____\\|/_____\\|/_____\\|/_____\\|/_____\\|");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(15, 7);
            Console.Write("\u2554\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2557");
            Console.SetCursorPosition(15, 8);
            Console.Write("\u2551            HackLife             \u2551");
            Console.SetCursorPosition(15, 9);
            Console.Write("\u2551                                 \u2551");
            Console.SetCursorPosition(15, 10);
            Console.Write("\u2551                                 \u2551");
            Console.SetCursorPosition(15, 11);
            Console.Write("\u2551   Play                          \u2551");
            Console.SetCursorPosition(15, 12);
            Console.Write("\u2551                                 \u2551");
            Console.SetCursorPosition(15, 13);
            Console.Write("\u2551   Settings                      \u2551");
            Console.SetCursorPosition(15, 14);
            Console.Write("\u2551                                 \u2551");
            Console.SetCursorPosition(15, 15);
            Console.Write("\u2551   Exit                          \u2551");
            Console.SetCursorPosition(15, 16);
            Console.Write("\u2551                                 \u2551");
            Console.SetCursorPosition(15, 17);
            Console.Write("\u255A\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u255D");
            int ci = 0;
            for(; ;)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                for (int i = 0; i < 6; i++)
                {
                    Console.SetCursorPosition(17, i + 11);
                    Console.Write(" ");
                }
                Console.SetCursorPosition(17, ci * 2 + 11);
                Console.Write("\u00BB");
                ConsoleKey k = Console.ReadKey(true).Key;
                if (k == ConsoleKey.DownArrow)
                {
                    ci += 1;
                    if (ci == 3) ci = 0;
                }
                else if (k == ConsoleKey.UpArrow)
                {
                    ci -= 1;
                    if (ci == -1) ci = 2;
                }
                else if (k == ConsoleKey.Enter)
                {
                    if (ci == 0)
                    {
                        Game.Start("game");
                        return;
                    }
                    else if (ci == 1)
                    {
                        return;
                    }
                    else if (ci == 2)
                    {
                        Console.Clear();
                        Environment.Exit(0);
                    }
                }
            }
        }
    }
}
