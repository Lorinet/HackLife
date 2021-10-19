using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    class MapGen
    {
        static int MapWidth = 500;
        static int MapHeight = 500;
        private Random RNG;
        private char[][] Map;
        public MapGen(int seed)
        {
            RNG = new Random(seed);
        }
        public char[][] Generate()
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
            Plains plainsGen = new Plains();
            Forest forestGen = new Forest();
            Desert desertGen = new Desert();
            Lake lakesGen = new Lake();
            BushField bushfieldGen = new BushField();
            WheatField wheatfieldGen = new WheatField();
            for (int i = 0; i < MapHeight; i++)
            {
                for (int j = 0; j < MapWidth; j++)
                {
                    if (RNG.Next(0, plainsGen.Rarity) == 1) plainsGen.Generate(j, i, RNG, ref Map);
                    if (RNG.Next(0, forestGen.Rarity) == 1) forestGen.Generate(j, i, RNG, ref Map);
                    if (RNG.Next(0, desertGen.Rarity) == 1) desertGen.Generate(j, i, RNG, ref Map);
                    if (RNG.Next(0, lakesGen.Rarity) == 1) lakesGen.Generate(j, i, RNG, ref Map);
                    if (RNG.Next(0, bushfieldGen.Rarity) == 1) bushfieldGen.Generate(j, i, RNG, ref Map);
                    if (RNG.Next(0, wheatfieldGen.Rarity) == 1) wheatfieldGen.Generate(j, i, RNG, ref Map);
                }
            }
            return Map;
        }
    }
    abstract class MapFeature
    {
        public MapFeature()
        {

        }
        public abstract int Rarity { get; }
        public void GenerateRandom(Random rng, ref char[][] Map)
        {
            int px = rng.Next(0, Map[0].Length);
            int py = rng.Next(0, Map.Length);
            Generate(px, py, rng, ref Map);
        }
        public abstract void Generate(int x, int y, Random rng, ref char[][] Map);
        public static void PutChar(int x, int y, char c, ref char[][] Map)
        {
            if (y >= 0 && y < Map.Length && x >= 0 && x < Map[0].Length) Map[y][x] = c;
        }
        public static void PutCharEx(int x, int y, char c, ref char[][] Map)
        {
            if(y >= 0 && y < Map.Length && x >= 0 && x < Map[0].Length)
            {
                if(Map[y][x] == '.')
                {
                    Map[y][x] = c;
                }
            }
        }
        public static void FillArea(char ch, int centerX, int centerY, int radius, ref char[][] Map)
        {
            int dist = 1;
            bool turn = false;
            for(int i = centerY - radius; i < centerY + radius; i++)
            {
                for(int j = centerX - dist; j < centerX + dist; j++)
                {
                    PutChar(j, i, ch, ref Map);
                }
                if (!turn) dist++;
                else dist--;
                if(dist == radius)
                {
                    dist--;
                    turn = true;
                }
            }
        }
        public static void FillAreaWithFeatures(MapFeature feat, int centerX, int centerY, int radius, int rarity, Random rng, ref char[][] Map)
        {
            int dist = 1;
            bool turn = false;
            for (int i = centerY - radius; i < centerY + radius; i++)
            {
                for (int j = centerX - dist; j < centerX + dist; j++)
                {
                    if (rng.Next(0, rarity) == 1)
                    {
                        feat.Generate(j, i, rng, ref Map);
                    }
                }
                if (!turn) dist++;
                else dist--;
                if (dist == radius)
                {
                    dist--;
                    turn = true;
                }
            }
        }
    }
    class Tree : MapFeature
    {
        public override int Rarity => 0;
        public override void Generate(int x, int y, Random rng, ref char[][] Map)
        {
            int height = rng.Next(2, 7);
            for(int i = y; i > y - height; i--)
            {
                PutChar(x, i, '▓', ref Map);
            }
            int[] posvX = { 0, -1, 1, -2, 2 };
            int[] posvY = { 0, 1, 2 };
            int leaves = rng.Next(1, 18);
            for(int i = 0; i < posvY.Length; i++)
            {
                for(int j = 0; j < posvX.Length; j++)
                {
                    PutChar(x + posvX[j], y - height - posvY[i], '#', ref Map);
                    leaves--;
                    if (leaves == 0) break;
                }
                if (leaves == 0) break;
            }
        }
    }
    class Rock : MapFeature
    {
        public override int Rarity => 0;
        public override void Generate(int x, int y, Random rng, ref char[][] Map)
        {
            int size = rng.Next(1, 3);
            FillArea('█', x, y, size, ref Map);
        }
    }
    class Bush : MapFeature
    {
        public override int Rarity => 0;
        public override void Generate(int x, int y, Random rng, ref char[][] Map)
        {
            int size = rng.Next(1, 4);
            for(int i = 0; i < 2; i++)
            {
                for(int j = 0; j < 2; j++)
                {
                    PutChar(x + j, y + i, 'Y', ref Map);
                    size--;
                    if (size == -1) return;
                }
            }
        }
    }
    class WheatCluster : MapFeature
    {
        public override int Rarity => 0;
        public override void Generate(int x, int y, Random rng, ref char[][] Map)
        {
            int size = rng.Next(2, 5);
            FillArea('¦', x, y, size, ref Map);
        }
    }
    class Bone : MapFeature
    {
        public override int Rarity => 0;
        public override void Generate(int x, int y, Random rng, ref char[][] Map)
        {
            int size = rng.Next(1, 4);
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    PutChar(x + j, y + i, '↔', ref Map);
                    size--;
                    if (size == -1) return;
                }
            }
        }
    }
    class Forest : MapFeature
    {
        public override int Rarity { get => 1100; }
        public override void Generate(int x, int y, Random rng, ref char[][] Map)
        {
            int size = rng.Next(100, 200);
            int rad = size / 2;
            FillArea('░', x, y, rad, ref Map);
            Tree treesGen = new Tree();
            FillAreaWithFeatures(treesGen, x, y, rad, 30, rng, ref Map);
            Rock rocksGen = new Rock();
            FillAreaWithFeatures(rocksGen, x, y, rad, 150, rng, ref Map);
        }
    }
    class Plains : MapFeature
    {
        public override int Rarity => 1000;
        public override void Generate(int x, int y, Random rng, ref char[][] Map)
        {
            int size = rng.Next(60, 200);
            int rad = size / 2;
            FillArea('‖', x, y, rad, ref Map);
            Tree treesGen = new Tree();
            FillAreaWithFeatures(treesGen, x, y, rad, 350, rng, ref Map);
            Bush bushGen = new Bush();
            FillAreaWithFeatures(bushGen, x, y, rad, 150, rng, ref Map);
        }
    }
    class Desert : MapFeature
    {
        public override int Rarity => 1150;
        public override void Generate(int x, int y, Random rng, ref char[][] Map)
        {
            int size = rng.Next(150, 400);
            int rad = size / 2;
            FillArea('▒', x, y, rad, ref Map);
            Bone bonesGen = new Bone();
            FillAreaWithFeatures(bonesGen, x, y, rad, 300, rng, ref Map);
            Bush bushGen = new Bush();
            FillAreaWithFeatures(bushGen, x, y, rad, 400, rng, ref Map);
        }
    }
    class BushField : MapFeature
    {
        public override int Rarity => 1050;
        public override void Generate(int x, int y, Random rng, ref char[][] Map)
        {
            int size = rng.Next(120, 250);
            int rad = size / 2;
            FillArea('‖', x, y, rad, ref Map);
            Bush bushGen = new Bush();
            FillAreaWithFeatures(bushGen, x, y, rad, 40, rng, ref Map);
        }
    }
    class WheatField : MapFeature
    {
        public override int Rarity => 1100;
        public override void Generate(int x, int y, Random rng, ref char[][] Map)
        {
            int size = rng.Next(60, 200);
            int rad = size / 2;
            FillArea('‖', x, y, rad, ref Map);
            Bush bushGen = new Bush();
            FillAreaWithFeatures(bushGen, x, y, rad, 40, rng, ref Map);
            WheatCluster wheatGen = new WheatCluster();
            FillAreaWithFeatures(wheatGen, x, y, rad, 40, rng, ref Map);
        }
    }

    class Lake : MapFeature
    {
        public override int Rarity => 1500;
        public override void Generate(int x, int y, Random rng, ref char[][] Map)
        {
            int size = rng.Next(60, 200);
            int rad = size / 2;
            FillArea('▒', x, y, rad + 3, ref Map);
            FillArea('~', x, y, rad, ref Map);
        }
    }
    class Sea : MapFeature
    {
        public override int Rarity => 3000;
        public override void Generate(int x, int y, Random rng, ref char[][] Map)
        {
            int size = rng.Next(300, 1000);
            int rad = size / 2;
            FillArea('~', x, y, rad, ref Map);
        }
    }
}
