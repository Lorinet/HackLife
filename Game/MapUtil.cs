using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Game
{
    static class MapUtil
    {
        public static void Load(string name)
        {
            string[] m = File.ReadAllLines(name + ".dat", Encoding.UTF8);
            Game.MapSize = new Point(m[0].Length, m.Length);
            Game.Map = new char[Game.MapSize.Y][];
            for (int i = 0; i < Game.MapSize.Y; i++)
            {
                Game.Map[i] = new char[Game.MapSize.X];
                for (int j = 0; j < Game.MapSize.X; j++)
                {
                    Game.Map[i][j] = (char)m[i][j];
                }
            }
            string[] playerdata = File.ReadAllLines(name + ".player.dat");
            string[] fl = playerdata[0].Split(' ');
            Game.Health = int.Parse(fl[0]);
            Game.PlayerPosition = new Point(int.Parse(fl[1]), int.Parse(fl[2]));
            Game.HandSlots = new int[5];
            string[] spl = playerdata[1].Split(' ');
            for(int i = 0; i < 5; i++)
            {
                Game.HandSlots[i] = int.Parse(spl[i]);
            }
            for(int i = 2; i < playerdata.Length; i++)
            {
                spl = playerdata[i].Split(' ');
                Game.Inventory.Add(int.Parse(spl[2]), new Item(Item.GlyphToName(spl[0][0]), int.Parse(spl[1]), spl[0][0], Item.GetSolidity(spl[0][0])));
            }
        }
        public static void Save(string name)
        {
            string[] lines = new string[Game.Map.Length];
            for(int i = 0; i < Game.Map.Length; i++)
            {
                lines[i] = "";
                for(int j = 0; j < Game.Map[0].Length; j++)
                {
                    lines[i] += Game.Map[i][j].ToString();
                }
            }
            File.WriteAllLines(name + ".dat", lines);
            string[] playerdata = new string[2 + Game.Inventory.Count];
            playerdata[0] = Game.Health.ToString() + " " + Game.PlayerPosition.X + " " + Game.PlayerPosition.Y;
            playerdata[1] = "";
            for (int i = 0; i < 5; i++) playerdata[1] += Game.HandSlots[i] + " ";
            List<Item> its = new List<Item>();
            foreach (KeyValuePair<int, Item> i in Game.Inventory) its.Add(i.Value);
            for (int i = 2; i < playerdata.Length; i++)
            {
                playerdata[i] = its[i - 2].Glyph + " " + its[i - 2].Count + " " + Game.GetInventoryID(its[i - 2].Glyph);
            }
            File.WriteAllLines(name + ".player.dat", playerdata);
        }
    }
}
