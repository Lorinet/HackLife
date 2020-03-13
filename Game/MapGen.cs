using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    class MapGen
    {
        static int MapWidth = 200;
        static int MapHeight = 100;
        private Random RNG { get; set; }
        private char[][] Map { get; set; }
        public MapGen(int seed)
        {
            RNG = new Random(seed);
        }
        public string[] Generate()
        {
            Map = new char[MapHeight][];
            for (int i = 0; i < MapHeight; i++)
            {
                Map[i] = new char[MapWidth];
                for (int j = 0; j < MapWidth; j++)
                {
                    Map[i][j] = '.';
                }
            }
            GenerateSpot(20, 10, 5, 'o');
            string[] gata = new string[MapHeight];
            for(int i = 0; i < MapHeight; i++)
            {
                gata[i] = "";
                for(int j = 0; j < MapWidth; j++)
                {
                    gata[i] += Map[i][j];
                }
            }
            return gata;
        }
        private void GenerateSpot(int x, int y, int radius, char glyph)
        {
            for (int i = 0; i < radius * 2; i++)
            {
                int dx = i - radius * 2 / 2;
                int tx = x + dx;
                int h = (int)Math.Round(radius * 2 * Math.Sqrt(radius * 2 * radius * 2 / 4.0 - dx * dx) / radius * 2);
                for (int dy = 1; dy <= h; dy++)
                {
                    if(y + dy < MapHeight) Map[y + dy][x] = glyph;
                    if(y - dy >= 0) Map[y - dy][x] = glyph;
                }
                if (h >= 0)
                {
                    Map[y][tx] = glyph;
                }
            }
        }
    }
}
