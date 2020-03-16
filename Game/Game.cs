using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Game
{
    static class Game
    {
        public static Point PlayerPosition { get; set; }
        public static Point MapSize { get; set; }
        public static char[][] Map { get; set; }
        public static bool Running { get; set; }
        public static ConsoleKey LastInput { get; set; }
        public static Dictionary<int, Item> Inventory { get; set; }
        public static int[] HandSlots { get; set; }
        public static int ActiveItemSlot { get; set; }
        public static int Health { get; set; }
        public static string WorldName { get; set; }
        public static Recipe[] HandCraftingRecipes { get; set; }
        public static Recipe[] CraftingRecipes { get; set; }
        public static Recipe[] FurnaceRecipes { get; set; }
        public static void Start(string mapfile)
        {
            Renderer.Initialize();
            PlayerPosition = new Point(0, 0);
            Health = 100;
            Inventory = new Dictionary<int, Item>();
            HandSlots = new int[5];
            MapUtil.Load(mapfile);
            HandCraftingRecipes = new Recipe[] { new Recipe(new[] { new Item(4, '▓') }, new Item(1, '◰')), new Recipe(new[] { new Item(8, '█') }, new Item(1, '◘')) };
            CraftingRecipes = new Recipe[]
            {
                new Recipe(new[] { new Item(4, '▓') }, new Item(1, '◰')),
                new Recipe(new[] { new Item(8, '█') }, new Item(1, '◘')),
                new Recipe(new[] { new Item(8, '▓') }, new Item(1, '⊠')),
                new Recipe(new[] { new Item(2, '▓') }, new Item(2, '|')),
                new Recipe(new[] { new Item(2, '|'), new Item(3, '▓') }, new Item(1, '┬')),
                new Recipe(new[] { new Item(2, '|'), new Item(3, '█') }, new Item(1, '┯')),
                new Recipe(new[] { new Item(2, '|'), new Item(3, '▃') }, new Item(1, '╤')),
                new Recipe(new[] { new Item(2, '|'), new Item(2, '█') }, new Item(1, '┍')),
                new Recipe(new[] { new Item(2, '|'), new Item(2, '▃') }, new Item(1, '╒')),
                new Recipe(new[] { new Item(1, '|'), new Item(2, '▃') }, new Item(1, '╀')),
                new Recipe(new[] { new Item(1, '|'), new Item(2, '◆') }, new Item(1, '╬')),
                new Recipe(new[] { new Item(2, '|'), new Item(4, '§') }, new Item(1, 'D')),
                new Recipe(new[] { new Item(1, '|'), new Item(1, '▃') }, new Item(4, '↟')),
                new Recipe(new[] { new Item(2, 'Y') }, new Item(1, '§')),
                new Recipe(new[] { new Item(10, '‖') }, new Item(1, '§')),
                new Recipe(new[] { new Item(2, '§') }, new Item(1, '▦')),
                new Recipe(new[] { new Item(4, '▦') }, new Item(1, '⊑')),
            };
            FurnaceRecipes = new Recipe[]
            {
                new Recipe(new[] { new Item(1, '▓') }, new Item(1, '⊚')),
                new Recipe(new[] { new Item(1, '⊡'), new Item(1, '⊚') }, new Item(1, '▃')),
                new Recipe(new[] { new Item(1, 'q'), new Item(1, '⊚') }, new Item(1, 'Q')),
            };
            Renderer.MapSection = new Point(PlayerPosition.X, PlayerPosition.Y);
            Renderer.PlayerScreenPosition = new Point(0, 0);
            WorldName = mapfile;
            Running = true;
            Renderer.RenderMap();
            Renderer.DrawPlayer();
            DrawInventorySlots();
            DrawHealth();
            while (Running) Cycle();
        }
        public static void Cycle()
        {
            UpdatePlayer();
        }
        public static void DrawHealth()
        {
            Console.SetCursorPosition(2, Renderer.MapSecHeight + 1);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("+");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" [..........] " + Health + "%");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(5, Renderer.MapSecHeight + 1);
            for (int i = 0; i < Health / 10; i++) Console.Write("#");
        }
        public static void DrawInventorySlots()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(2, Renderer.MapSecHeight + 3);
            string sp = "";
            for (int i = 0; i < Console.BufferWidth - 4; i++) sp += " ";
            Console.Write(sp);
            Console.SetCursorPosition(2, Renderer.MapSecHeight + 3);
            string w = "";
            for (int i = 0; i < HandSlots.Length; i++)
            {
                if (Inventory.ContainsKey(HandSlots[i]))
                {
                    if (i == ActiveItemSlot) w += '\u00BB';
                    else w += ' ';
                    w += Inventory[HandSlots[i]].Glyph;
                    w += " ";
                    w += Inventory[HandSlots[i]].Count;
                    w += " ";
                    w += Inventory[HandSlots[i]].Name;
                    w += " |";
                }
                else if (HandSlots[i] == -1)
                {
                    w += " <Empty> |";
                }
                else HandSlots[i] = -1;
            }
            w = w.Remove(w.Length - 2);
            Console.WriteLine(w);
        }
        public static void UpdatePlayer()
        {
            try
            {
                LastInput = Console.ReadKey(true).Key;
                Console.ForegroundColor = ConsoleColor.Green;
                HandleBreakPlace();
                Console.SetCursorPosition(Renderer.PlayerScreenPosition.X, Renderer.PlayerScreenPosition.Y);
                switch (LastInput)
                {
                    case ConsoleKey.Escape:
                        Menu();
                        break;
                    case ConsoleKey.I:
                        ShowInventory(Inventory);
                        break;
                    case ConsoleKey.C:
                        Crafting(HandCraftingRecipes, "Crafting");
                        break;
                    case ConsoleKey.W:
                        if (!Obstacle(Renderer.Screen[Renderer.PlayerScreenPosition.Y - 1][Renderer.PlayerScreenPosition.X]))
                        {
                            Console.Write(Renderer.Screen[Renderer.PlayerScreenPosition.Y][Renderer.PlayerScreenPosition.X]);
                            PlayerPosition.Y -= 1;
                        }
                        break;
                    case ConsoleKey.S:
                        if (PlayerPosition.Y < Map.Length - 1)
                        {
                            if (!Obstacle(Renderer.Screen[Renderer.PlayerScreenPosition.Y + 1][Renderer.PlayerScreenPosition.X]))
                            {
                                Console.Write(Renderer.Screen[Renderer.PlayerScreenPosition.Y][Renderer.PlayerScreenPosition.X]);
                                PlayerPosition.Y += 1;
                            }
                        }
                        break;
                    case ConsoleKey.A:
                        if (!Obstacle(Renderer.Screen[Renderer.PlayerScreenPosition.Y][Renderer.PlayerScreenPosition.X - 1]))
                        {
                            Console.Write(Renderer.Screen[Renderer.PlayerScreenPosition.Y][Renderer.PlayerScreenPosition.X]);
                            PlayerPosition.X -= 1;
                        }
                        break;
                    case ConsoleKey.D:
                        if (PlayerPosition.X < Map[0].Length - 1)
                        {
                            if (!Obstacle(Renderer.Screen[Renderer.PlayerScreenPosition.Y][Renderer.PlayerScreenPosition.X + 1]))
                            {
                                Console.Write(Renderer.Screen[Renderer.PlayerScreenPosition.Y][Renderer.PlayerScreenPosition.X]);
                                PlayerPosition.X += 1;
                            }
                        }
                        break;
                }
                if (PlayerPosition.X < Map[0].Length - 1) Renderer.PlayerScreenPosition.X = PlayerPosition.X - Renderer.MapSection.X;
                else PlayerPosition.X = Map[0].Length - 2;
                if (PlayerPosition.Y < Map.Length - 1) Renderer.PlayerScreenPosition.Y = PlayerPosition.Y - Renderer.MapSection.Y;
                else PlayerPosition.Y = Map.Length - 2;
                Renderer.DrawPlayer();
            }
            catch (Exception ex)
            {
                if (ex.GetType().ToString() == "System.IndexOutOfRangeException")
                {
                    if (PlayerPosition.Y > 0)
                    {
                        if (LastInput == ConsoleKey.W || LastInput == ConsoleKey.S)
                        {
                            if (PlayerPosition.Y == Renderer.MapSection.Y)
                            {
                                if (!Obstacle(Map[PlayerPosition.Y - 1][PlayerPosition.X]))
                                {
                                    Renderer.MapSection.Y -= 1;
                                    Renderer.PlayerScreenPosition.Y = 0;
                                    PlayerPosition.Y -= 1;
                                }
                                for (int i = 0; i < 3; i++)
                                {
                                    if (Renderer.MapSection.Y > 0)
                                    {
                                        Renderer.MapSection.Y -= 1;
                                        Renderer.PlayerScreenPosition.Y += 1;
                                    }
                                }
                                Renderer.RenderMap();
                                Renderer.DrawPlayer();
                            }
                            else if (PlayerPosition.Y == Renderer.MapSection.Y + Renderer.MapSecHeight - 1)
                            {
                                if (!Obstacle(Map[PlayerPosition.Y + 1][PlayerPosition.X]))
                                {
                                    Renderer.MapSection.Y += 1;
                                    Renderer.PlayerScreenPosition.Y = Renderer.MapSecHeight - 1;
                                    PlayerPosition.Y += 1;
                                }
                                for (int i = 0; i < 3; i++)
                                {
                                    if (Renderer.MapSection.Y + Renderer.MapSecHeight < Map.Length - 1)
                                    {
                                        Renderer.MapSection.Y += 1;
                                        Renderer.PlayerScreenPosition.Y -= 1;
                                    }
                                }
                                Renderer.RenderMap();
                                Renderer.DrawPlayer();
                            }
                        }
                    }
                    if (PlayerPosition.X > 0)
                    {
                        if (LastInput == ConsoleKey.A || LastInput == ConsoleKey.D)
                        {
                            if (PlayerPosition.X == Renderer.MapSection.X)
                            {
                                if (!Obstacle(Map[PlayerPosition.Y][PlayerPosition.X - 1]))
                                {
                                    Renderer.MapSection.X -= 1;
                                    Renderer.PlayerScreenPosition.X = 0;
                                    PlayerPosition.X -= 1;
                                }
                                for (int i = 0; i < 3; i++)
                                {
                                    if (Renderer.MapSection.X > 0)
                                    {
                                        Renderer.MapSection.X -= 1;
                                        Renderer.PlayerScreenPosition.X += 1;
                                    }
                                }
                                Renderer.RenderMap();
                                Renderer.DrawPlayer();
                            }
                            else if (PlayerPosition.X == Renderer.MapSection.X + Renderer.MapSecWidth - 1)
                            {
                                if (!Obstacle(Map[PlayerPosition.Y][PlayerPosition.X + 1]) && PlayerPosition.X < Map[0].Length - 1)
                                {
                                    Renderer.MapSection.X += 1;
                                    Renderer.PlayerScreenPosition.X = Renderer.MapSecWidth - 1;
                                    PlayerPosition.X += 1;
                                }
                                for (int i = 0; i < 3; i++)
                                {
                                    if (Renderer.MapSection.X + Renderer.MapSecWidth < Map[0].Length - 1)
                                    {
                                        Renderer.MapSection.X += 1;
                                        Renderer.PlayerScreenPosition.X -= 1;
                                    }
                                }
                                Renderer.RenderMap();
                                Renderer.DrawPlayer();
                            }
                        }
                    }
                }
            }
        }
        public static void HandleBreakPlace()
        {
            int xm = 0;
            int ym = 0;
            bool act = false;
            switch (LastInput)
            {
                case ConsoleKey.D1:
                    ActiveItemSlot = 0;
                    DrawInventorySlots();
                    break;
                case ConsoleKey.D2:
                    ActiveItemSlot = 1;
                    DrawInventorySlots();
                    break;
                case ConsoleKey.D3:
                    ActiveItemSlot = 2;
                    DrawInventorySlots();
                    break;
                case ConsoleKey.D4:
                    ActiveItemSlot = 3;
                    DrawInventorySlots();
                    break;
                case ConsoleKey.D5:
                    ActiveItemSlot = 4;
                    DrawInventorySlots();
                    break;
                case ConsoleKey.UpArrow:
                    ym = -1;
                    act = true;
                    break;
                case ConsoleKey.DownArrow:
                    ym = 1;
                    act = true;
                    break;
                case ConsoleKey.LeftArrow:
                    xm = -1;
                    act = true;
                    break;
                case ConsoleKey.RightArrow:
                    xm = 1;
                    act = true;
                    break;
            }
            if(act)
            {
                if (Renderer.PlayerScreenPosition.X + xm >= 0 && Renderer.PlayerScreenPosition.X + xm < Renderer.MapSecWidth && Renderer.PlayerScreenPosition.Y + ym >= 0 && Renderer.PlayerScreenPosition.Y + ym < Renderer.MapSecHeight)
                {
                    Console.SetCursorPosition(22, Renderer.MapSecHeight + 1);
                    Console.Write(Item.GlyphToName(Renderer.Screen[Renderer.PlayerScreenPosition.Y + ym][Renderer.PlayerScreenPosition.X + xm]));
                    Console.SetCursorPosition(Renderer.PlayerScreenPosition.X + xm, Renderer.PlayerScreenPosition.Y + ym);
                    char block = Renderer.Screen[Renderer.PlayerScreenPosition.Y + ym][Renderer.PlayerScreenPosition.X + xm];
                    if (block == '◰')
                    {
                        Crafting(CraftingRecipes, "Workbench");
                        return;
                    }
                    else if(block == '◘')
                    {
                        Crafting(FurnaceRecipes, "Furnace");
                        return;
                    }
                    Console.Write("*");
                    ConsoleKey brk = Console.ReadKey(true).Key;
                    if (brk == ConsoleKey.X)
                    {
                        if (block != '.')
                        {
                            Item i;
                            if (HandSlots[ActiveItemSlot] == -1) i = Item.HandleBreak(block, '.');
                            else i = Item.HandleBreak(block, Inventory[HandSlots[ActiveItemSlot]].Glyph);
                            if(i.Glyph != '.')
                            {
                                Renderer.Screen[Renderer.PlayerScreenPosition.Y + ym][Renderer.PlayerScreenPosition.X + xm] = '.';
                                Map[PlayerPosition.Y + ym][PlayerPosition.X + xm] = '.';
                                Console.SetCursorPosition(Renderer.PlayerScreenPosition.X + xm, Renderer.PlayerScreenPosition.Y + ym);
                                Console.Write(".");
                                AddItem(i);
                                Inventory[HandSlots[ActiveItemSlot]].Durability -= 1;
                                if(Inventory[HandSlots[ActiveItemSlot]].Durability == 0)
                                {
                                    Inventory.Remove(HandSlots[ActiveItemSlot]);
                                    HandSlots[ActiveItemSlot] = -1;
                                }
                            }
                        }
                        Console.SetCursorPosition(22, Renderer.MapSecHeight + 1);
                        string spc = "";
                        for (int i = 0; i < Renderer.MapSecWidth - 25; i++) spc += " ";
                        Console.Write(spc);
                        Renderer.RenderMap();
                    }
                    else if (brk == ConsoleKey.E && Renderer.Screen[Renderer.PlayerScreenPosition.Y + ym][Renderer.PlayerScreenPosition.X + xm] == '.' && HandSlots[ActiveItemSlot] != -1)
                    {
                        Renderer.Screen[Renderer.PlayerScreenPosition.Y + ym][Renderer.PlayerScreenPosition.X + xm] = Inventory[HandSlots[ActiveItemSlot]].Glyph;
                        Map[PlayerPosition.Y + ym][PlayerPosition.X + xm] = Inventory[HandSlots[ActiveItemSlot]].Glyph;
                        Inventory[HandSlots[ActiveItemSlot]].Count--;
                        Renderer.RenderMap();
                        if (Inventory[HandSlots[ActiveItemSlot]].Count == 0)
                        {
                            Inventory.Remove(HandSlots[ActiveItemSlot]);
                            HandSlots[ActiveItemSlot] = -1;
                        }
                        DrawInventorySlots();
                    }
                    else
                    {
                        Console.SetCursorPosition(Renderer.PlayerScreenPosition.X + xm, Renderer.PlayerScreenPosition.Y + ym);
                        Console.Write(Renderer.Screen[Renderer.PlayerScreenPosition.Y + ym][Renderer.PlayerScreenPosition.X + xm]);
                        Console.SetCursorPosition(22, Renderer.MapSecHeight + 1);
                        string spc = "";
                        for (int i = 0; i < Renderer.MapSecWidth - 25; i++) spc += " ";
                        Console.Write(spc);
                    }
                }
            }
        }
        public static void Menu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(3, 1);
            Console.Write("\u2554\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2557");
            Console.SetCursorPosition(3, 2);
            Console.Write("\u2551                  \u2551");
            Console.SetCursorPosition(3, 3);
            Console.Write("\u2551   Game Paused    \u2551");
            Console.SetCursorPosition(3, 4);
            Console.Write("\u2551                  \u2551");
            Console.SetCursorPosition(3, 5);
            Console.Write("\u2551                  \u2551");
            Console.SetCursorPosition(3, 6);
            Console.Write("\u2551   Resume game    \u2551");
            Console.SetCursorPosition(3, 7);
            Console.Write("\u2551   Save game      \u2551");
            Console.SetCursorPosition(3, 8);
            Console.Write("\u2551   Exit           \u2551");
            Console.SetCursorPosition(3, 9);
            Console.Write("\u2551                  \u2551");
            Console.SetCursorPosition(3, 10);
            Console.Write("\u255A\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u255D");
            int ci = 0;
            for (; ; )
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                for (int i = 0; i < 3; i++)
                {
                    Console.SetCursorPosition(5, i + 6);
                    Console.Write(" ");
                }
                Console.SetCursorPosition(5, ci + 6);
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
                        Renderer.RenderMap();
                        Renderer.DrawPlayer();
                        return;
                    }
                    else if (ci == 1)
                    {
                        MapUtil.Save(WorldName);
                        Renderer.RenderMap();
                        Renderer.DrawPlayer();
                        return;
                    }
                    else if (ci == 2)
                    {
                        Running = false;
                        return;
                    }
                }
                else if (k == ConsoleKey.Escape)
                {
                    Renderer.RenderMap();
                    Renderer.DrawPlayer();
                    return;
                }
            }
        }
        public static void ShowInventory(Dictionary<int, Item> inv)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(3, 1);
            Console.Write("\u2554\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2557");
            Console.SetCursorPosition(3, 2);
            Console.Write("\u2551 Inventory                       \u2551");
            Console.SetCursorPosition(3, 3);
            Console.Write("\u2551                                 \u2551");
            Console.SetCursorPosition(3, 4);
            Console.Write("\u2551                                 \u2551");
            List<Item> its = new List<Item>();
            foreach (KeyValuePair<int, Item> i in inv) its.Add(i.Value);
            if (its.Count == 0)
            {
                Renderer.RenderMap();
                Renderer.DrawPlayer();
                return;
            }
            for (int i = 5; i < 5 + inv.Count; i++)
            {
                Console.SetCursorPosition(3, i);
                Console.Write("\u2551                                 \u2551");
                Console.SetCursorPosition(4, i);
                Console.Write("   " + its[i - 5].Glyph + " " + its[i - 5].Count + " x " + its[i - 5].Name + ((its[i - 5].Durability == -1) ? "" : " (" + its[i - 5].Durability + " D)"));
            }
            Console.SetCursorPosition(3, 5 + its.Count);
            Console.Write("\u2551                                 \u2551");
            Console.SetCursorPosition(3, 6 + its.Count);
            Console.Write("\u255A\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u255D");
            int ci = 0;
            for (; ; )
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                for (int i = 0; i < its.Count; i++)
                {
                    Console.SetCursorPosition(5, i + 5);
                    Console.Write(" ");
                }
                Console.SetCursorPosition(5, ci + 5);
                Console.Write("\u00BB");
                ConsoleKey k = Console.ReadKey(true).Key;
                if (k == ConsoleKey.DownArrow)
                {
                    ci += 1;
                    if (ci == its.Count) ci = 0;
                }
                else if (k == ConsoleKey.UpArrow)
                {
                    ci -= 1;
                    if (ci == -1) ci = its.Count - 1;
                }
                else if (k == ConsoleKey.D1)
                {
                    HandSlots[0] = GetInventoryID(its[ci].Glyph);
                    DrawInventorySlots();
                }
                else if (k == ConsoleKey.D2)
                {
                    HandSlots[1] = GetInventoryID(its[ci].Glyph);
                    DrawInventorySlots();
                }
                else if (k == ConsoleKey.D3)
                {
                    HandSlots[2] = GetInventoryID(its[ci].Glyph);
                    DrawInventorySlots();
                }
                else if (k == ConsoleKey.D4)
                {
                    HandSlots[3] = GetInventoryID(its[ci].Glyph);
                    DrawInventorySlots();
                }
                else if (k == ConsoleKey.D5)
                {
                    HandSlots[4] = GetInventoryID(its[ci].Glyph);
                    DrawInventorySlots();
                }
                else if (k == ConsoleKey.Escape)
                {
                    Renderer.RenderMap();
                    Renderer.DrawPlayer();
                    return;
                }
            }
        }
        public static void Crafting(Recipe[] recs, string utilTitle)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(3, 1);
            Console.Write("\u2554\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2557");
            Console.SetCursorPosition(3, 2);
            Console.Write("\u2551                                                \u2551");
            Console.SetCursorPosition(5, 2);
            Console.Write(utilTitle);
            Console.SetCursorPosition(3, 3);
            Console.Write("\u2551                                                \u2551");
            Console.SetCursorPosition(3, 4);
            Console.Write("\u2551                                                \u2551");
            int rw = 0;
            int le = 5;
            if (recs.Length < le) le = recs.Length;
            for (int i = rw; i < rw + le; i++)
            {
                Console.SetCursorPosition(3, 5 + i - rw);
                Console.Write("\u2551                                                \u2551");
                Console.SetCursorPosition(5, 5 + i - rw);
                Console.Write(recs[i].Result.Glyph + " " + recs[i].Result.Name);
            }
            Console.SetCursorPosition(3, 5 + le);
            Console.Write("\u2551                                                \u2551");
            Console.SetCursorPosition(3, 6 + le);
            Console.Write("\u255A\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u255D");
            int ci = 0;
            int sci = 0;
            for (; ; )
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                for (int i = rw; i < rw + le; i++)
                {
                    Console.SetCursorPosition(3, 5 + i - rw);
                    Console.Write("\u2551                                                \u2551");
                    Console.SetCursorPosition(7, 5 + i - rw);
                    string strIng = "";
                    foreach (Item it in recs[i].Ingredients) strIng += it.Count.ToString() + it.Glyph.ToString() + " ";
                    Console.Write(recs[i].Result.Glyph + " " + recs[i].Result.Name + "  (" + strIng.Remove(strIng.Length - 1) + ")");
                }
                for (int i = 0; i < le; i++)
                {
                    Console.SetCursorPosition(5, i + 5);
                    Console.Write(" ");
                }
                Console.SetCursorPosition(5, sci + 5);
                Console.Write("\u00BB");
                ConsoleKey k = Console.ReadKey(true).Key;
                if (k == ConsoleKey.DownArrow)
                {
                    ci += 1;
                    sci += 1;
                    if (sci == le)
                    {
                        rw++;
                        sci = le - 1;
                    }
                    if (ci == recs.Length)
                    {
                        ci = 0;
                        rw = 0;
                        sci = 0;
                    }
                }
                else if (k == ConsoleKey.UpArrow)
                {
                    ci -= 1;
                    sci -= 1;
                    if (sci == -1)
                    {
                        rw--;
                        sci = 0;
                    }
                    if (ci == -1)
                    {
                        ci = recs.Length - 1;
                        rw = recs.Length - le;
                        sci = le - 1;
                    }
                }
                else if (k == ConsoleKey.Enter)
                {
                    Renderer.RenderMap();
                    Renderer.DrawPlayer();
                    Craft(recs[ci]);
                    Renderer.RenderMap();
                    Renderer.DrawPlayer();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(3, 1);
                    Console.Write("\u2554\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2557");
                    Console.SetCursorPosition(3, 2);
                    Console.Write("\u2551                                                \u2551");
                    Console.SetCursorPosition(5, 2);
                    Console.Write(utilTitle);
                    Console.SetCursorPosition(3, 3);
                    Console.Write("\u2551                                                \u2551");
                    Console.SetCursorPosition(3, 4);
                    Console.Write("\u2551                                                \u2551");
                    Console.SetCursorPosition(3, 5 + le);
                    Console.Write("\u2551                                                \u2551");
                    Console.SetCursorPosition(3, 6 + le);
                    Console.Write("\u255A\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u255D");
                }
                else if (k == ConsoleKey.Escape)
                {
                    Renderer.RenderMap();
                    Renderer.DrawPlayer();
                    return;
                }
            }
        }
        public static void Craft(Recipe recp)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(3, 1);
            Console.Write("\u2554\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2557");
            Console.SetCursorPosition(3, 2);
            Console.Write("\u2551                                                \u2551");
            Console.SetCursorPosition(5, 2);
            Console.Write(recp.Result.Glyph + " " + recp.Result.Name);
            Console.SetCursorPosition(3, 3);
            Console.Write("\u2551                                                \u2551");
            Console.SetCursorPosition(3, 4);
            Console.Write("\u2551 Required items:                                \u2551");
            Console.SetCursorPosition(3, 5 + recp.Ingredients.Length);
            Console.Write("\u2551                                                \u2551");
            Console.SetCursorPosition(3, 6 + recp.Ingredients.Length);
            Console.Write("\u2551 Press Enter to craft and Escape to exit        \u2551");
            Console.SetCursorPosition(3, 7 + recp.Ingredients.Length);
            Console.Write("\u2551                                                \u2551");
            Console.SetCursorPosition(3, 8 + recp.Ingredients.Length);
            Console.Write("\u255A\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u255D");
            for(; ;)
            {
                bool enough = true;
                for (int i = 0; i < recp.Ingredients.Length; i++)
                {
                    Console.SetCursorPosition(3, 5 + i);
                    Console.Write("\u2551                                                \u2551");
                    Console.SetCursorPosition(5, 5 + i);
                    int nd = GetInventoryID(recp.Ingredients[i].Glyph);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (nd == -1 || Inventory[nd].Count < recp.Ingredients[i].Count)
                    {
                        enough = false;
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.Write(recp.Ingredients[i].Glyph + " " + recp.Ingredients[i].Name + " x " + recp.Ingredients[i].Count);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                ConsoleKey ke = Console.ReadKey(true).Key;
                if (ke == ConsoleKey.Enter)
                {
                    if (enough)
                    {
                        foreach (Item it in recp.Ingredients)
                        {
                            Inventory[GetInventoryID(it.Glyph)].Count -= it.Count;
                        }
                        AddItem(recp.Result);
                    }
                    else return;
                }
                else if (ke == ConsoleKey.Escape) return;
            }
        }
        public static int GetInventoryID(char glyph)
        {
            foreach (KeyValuePair<int, Item> item in Inventory)
            {
                if (item.Value.Glyph == glyph) return item.Key;
            }
            return -1;
        }
        public static int AddItem(Item itm)
        {
            int iid = GetInventoryID(itm.Glyph);
            if (iid == -1)
            {
                int index = 0;
                while (Inventory.ContainsKey(index)) index++;
                Inventory.Add(index, new Item(itm.Count, itm.Glyph));
                DrawInventorySlots();
                return index;
            }
            else
            {
                Inventory[iid].Count += itm.Count;
                DrawInventorySlots();
                return iid;
            }
        }
        public static bool Obstacle(char b)
        {
            return Item.GetSolidity(b);
        }
    }
    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
