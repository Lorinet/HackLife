using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Game
{
    class Program
    {
        static Random rng = new Random();
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
                        SelectWorld();
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
        static void SelectWorld()
        {
            Console.Clear();
            string[] sf = MapUtil.GetSaves();
            Console.SetWindowSize(66, sf.Length + 7);
            Console.SetBufferSize(66, sf.Length + 7);
            Console.SetCursorPosition(15, 1);
            Console.Write("\u2554\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2557");
            Console.SetCursorPosition(15, 2);
            Console.Write("\u2551           Select world          \u2551");
            Console.SetCursorPosition(15, 3);
            Console.Write("\u2551                                 \u2551");
            for (int i = 0; i < sf.Length; i++)
            {
                Console.SetCursorPosition(15, 4 + i);
                Console.Write("\u2551                                 \u2551");
                Console.SetCursorPosition(19, 4 + i);
                if (sf[i] != "new")
                    Console.Write(sf[i]);
                else Console.Write("Create new world");
            }
            Console.SetCursorPosition(15, 4 + sf.Length);
            Console.Write("\u2551                                 \u2551");
            Console.SetCursorPosition(15, 5 + sf.Length);
            Console.Write("\u255A\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u255D");
            int ci = 0;
            for(; ; )
            {
                for(int i = 0; i < sf.Length; i++)
                {
                    Console.SetCursorPosition(17, 4 + i);
                    Console.Write(" ");
                }
                Console.SetCursorPosition(17, 4 + ci);
                Console.Write("\u00BB"); ;
                ConsoleKey ck = Console.ReadKey(true).Key;
                if(ck == ConsoleKey.DownArrow)
                {
                    ci++;
                    if (ci == sf.Length) ci = 0;
                }
                else if(ck == ConsoleKey.UpArrow)
                {
                    ci--;
                    if (ci == -1) ci = sf.Length - 1;
                }
                else if(ck == ConsoleKey.Enter)
                {
                    if (sf[ci] != "new")
                        Game.Start(sf[ci]);
                    else
                        NewWorld();
                    return;
                }
                else if(ck == ConsoleKey.Escape)
                {
                    return;
                }
            }
        }
        static void NewWorld()
        {
            Console.SetWindowSize(66, 10);
            Console.SetBufferSize(66, 10);
            Console.Clear();
            Console.SetCursorPosition(15, 1);
            Console.Write("\u2554\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2557");
            Console.SetCursorPosition(15, 2);
            Console.Write("\u2551           Create world          \u2551");
            Console.SetCursorPosition(15, 3);
            Console.Write("\u2551                                 \u2551");
            Console.SetCursorPosition(15, 4);
            Console.Write("\u2551 Name:                           \u2551");
            Console.SetCursorPosition(15, 5);
            Console.Write("\u2551                                 \u2551");
            Console.SetCursorPosition(15, 6);
            Console.Write("\u255A\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u255D");
            getname:
            Console.SetCursorPosition(23, 4);
            Console.Write("                         ");
            Console.SetCursorPosition(23, 4);
            Console.CursorVisible = true;
            string name = Console.ReadLine();
            Console.CursorVisible = false;
            if(name == "new" || name.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(17, 8);
                Console.Write("Invalid name!");
                Console.ForegroundColor = ConsoleColor.Yellow;
                goto getname;
            }
            Console.SetCursorPosition(17, 8);
            Console.Write("Generating world...");
            MapGen mapgen = new MapGen(rng.Next(0, 9999999));
            Directory.CreateDirectory("saves\\" + name);
            char[][] map = mapgen.Generate();
            StreamWriter w = new StreamWriter("saves\\" + name + "\\" + name + ".dat");
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[0].Length; j++)
                {
                    w.Write(map[i][j]);
                }
                w.Write('\n');
            }
            w.Close();
            string[] playerdata = 
            {
                "100 0 0",
                "-1 -1 -1 -1 -1"
            };
            File.WriteAllLines("saves\\" + name + "\\" + name + ".player.dat", playerdata);
            Game.Start(name);
        }
    }
}
