using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    class Item
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public char Glyph { get; set; }
        public bool Block { get; set; }
        public Item(string name, int count, char glyph, bool block)
        {
            Name = name;
            Count = count;
            Glyph = glyph;
            Block = block;
        }
        public static string GlyphToName(char glyph)
        {
            switch(glyph)
            {
                case '.':
                    return "Air";
                case '#':
                    return "Stone";
                case '@':
                    return "Apple";
                case '\u2583':
                    return "Iron Ingot";
                case '\u2591':
                    return "Sand";
                case '\u2593':
                    return "Bricks";
            }
            return "Air";
        }
        public static bool GetSolidity(char glyph)
        {
            switch (glyph)
            {
                case '.':
                    return false;
                case '#':
                    return true;
                case '@':
                    return false;
                case '\u2583':
                    return false;
                case '\u2591':
                    return false;
                case '\u2593':
                    return true;
            }
            return false;
        }
    }
    class Recipe
    {
        public Item[] Ingredients { get; set; }
        public Item Result { get; set; }
        public Recipe(Item[] ing, Item res)
        {
            Ingredients = ing;
            Result = res;
        }
    }
}
