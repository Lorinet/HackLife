using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Game
{
    static class MapUtil
    {
        public static void Load(string name)
        {
            //string[] m = Unzip(File.ReadAllBytes("saves\\" + name + "\\" + name + ".dat")).Split('\n');
            string[] m = File.ReadAllLines("saves\\" + name + "\\" + name + ".dat");
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
            string[] playerdata = File.ReadAllLines("saves\\" + name + "\\" + name + ".player.dat");
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
                Game.Inventory.Add(int.Parse(spl[2]), new Item(int.Parse(spl[1]), spl[0][0]));
            }
        }
        public static void Save(string name)
        {
            //string lines = "";
            //for(int i = 0; i < Game.Map.Length; i++)
            //{
            //    for(int j = 0; j < Game.Map[0].Length; j++)
            //    {
            //        lines += Game.Map[i][j].ToString();
            //    }
            //    lines += "\n";
            //}
            //lines.Remove(lines.Length - 1);
            //File.WriteAllBytes("saves\\" + name + "\\" + name + ".dat", Zip(lines));
            StreamWriter w = new StreamWriter("saves\\" + name + "\\" + name + ".dat", false);
            for (int i = 0; i < Game.Map.Length; i++)
            {
                for (int j = 0; j < Game.Map[0].Length; j++)
                {
                    w.Write(Game.Map[i][j]);
                }
                w.Write('\n');
            }
            w.Close();
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
            File.WriteAllLines("saves\\" + name + "\\" + name + ".player.dat", playerdata);
        }
        public static string[] GetSaves()
        {
            List<string> l = new List<string>(Directory.GetDirectories("saves"));
            for (int i = 0; i < l.Count; i++) l[i] = Path.GetFileName(l[i]);
            l.Add("new");
            return l.ToArray();
        }
        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];
            int cnt;
            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }
        public static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    CopyTo(msi, gs);
                }
                return mso.ToArray();
            }
        }
        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    CopyTo(gs, mso);
                }
                string s = Encoding.UTF8.GetString(mso.ToArray());
                return s.Remove(s.Length - 1);
            }
        }
    }
}
