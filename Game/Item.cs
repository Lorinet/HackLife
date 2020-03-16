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
        public int Durability { get; set; }
        public Item(int count, char glyph)
        {
            Name = GlyphToName(glyph);
            Count = count;
            Glyph = glyph;
            Block = GetSolidity(glyph);
            Durability = GetItemDurability(glyph);
        }
        public static string GlyphToName(char glyph)
        {
            switch(glyph)
            {
                case '.':
                    return "Air";
                case '#':
                    return "Leaf";
                case 'o':
                    return "Apple";
                case '⊡':
                    return "Iron Ore";
                case '▃':
                    return "Iron Ingot";
                case '◆':
                    return "Diamond";
                case '░':
                    return "Dirt";
                case '▒':
                    return "Sand";
                case '▓':
                    return "Wood";
                case '◰':
                    return "Workbench";
                case '◘':
                    return "Furnace";
                case '~':
                    return "Water";
                case '█':
                    return "Stone";
                case '⌧':
                    return "Glass";
                case '▚':
                    return "Bricks";
                case '‖':
                    return "Grass";
                case 'Y':
                    return "Hemp";
                case '↔':
                    return "Bone";
                case '¦':
                    return "Wheat";
                case '↟':
                    return "Arrow";
                case 'D':
                    return "Bow";
                case '§':
                    return "String";
                case '|':
                    return "Stick";
                case '╀':
                    return "Iron Sword";
                case '╬':
                    return "Diamond Sword";
                case '┬':
                    return "Wooden Pickaxe";
                case '┯':
                    return "Stone Pickaxe";
                case '╤':
                    return "Iron Pickaxe";
                case '┍':
                    return "Stone Axe";
                case '╒':
                    return "Iron Axe";
                case '⊟':
                    return "Iron Armor";
                case '⊞':
                    return "Diamond Armor";
                case '▣':
                    return "Diamond Block";
                case '⊑':
                    return "Shirt";
                case '▦':
                    return "Cloth";
                case '⊠':
                    return "Chest";
                case '⊚':
                    return "Charcoal";
                case 'q':
                    return "Raw Meat";
                case 'Q':
                    return "Steak";
            }
            return "Air";
        }
        public static bool GetEdible(char item)
        {
            switch(item)
            {
                case '@':
                    return true;
                case 'Q':
                    return true;
            }
            return false;
        }
        public static bool GetSolidity(char glyph)
        {
            switch (glyph)
            {
                case '⊡':
                    return true;
                case '▓':
                    return true;
                case '◰':
                    return true;
                case '◘':
                    return true;
                case '█':
                    return true;
                case '▯':
                    return true;
                case '▚':
                    return true;
            }
            return false;
        }
        public static int GetStrength(char item)
        {
            switch(item)
            {
                case '┬':
                    return 1;
                case '┯':
                    return 2;
                case '╤':
                    return 3;
            }
            return 0;
        }
        public static int GetItemDurability(char item)
        {
            switch (item)
            {
                case '┬':
                    return 20;
                case '┯':
                    return 50;
                case '╤':
                    return 200;
                case '╀':
                    return 70;
                case '╬':
                    return 200;
                case '┍':
                    return 100;
                case '╒':
                    return 200;
            }
            return -1;
        }
        public static int GetDurability(char glyph)
        {
            switch(glyph)
            {
                case '◘':
                    return 1;
                case '█':
                    return 1;
                case '⊡':
                    return 2;
                case '▚':
                    return 2;
                case '▣':
                    return 3;
            }
            return 0;
        }
        public static Item HandleBreak(char glyph, char tool)
        {
            if(GetStrength(tool) >= GetDurability(glyph))
            {
                return new Item(1, glyph);
            }
            else return new Item(0, '.');
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
