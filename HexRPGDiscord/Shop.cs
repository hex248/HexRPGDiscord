using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexRPGDiscord
{
    class Shop
    {
        public List<string> moves = new List<string>();
        public List<int> movesprice = new List<int>();

        public Shop(string[] moveset)
        {
            if (!moveset.Contains("Firebolt"))
            {
                moves.Add("Firebolt");
                movesprice.Add(30);
            }
            if (!moveset.Contains("Piercing Arrow"))
            {
                moves.Add("Piercing Arrow");
                movesprice.Add(30);
            }
            if (!moveset.Contains("Punch"))
            {
                moves.Add("Punch");
                movesprice.Add(30);
            }
            if (!moveset.Contains("Kick"))
            {
                moves.Add("Kick");
                movesprice.Add(55);
            }
            if (!moveset.Contains("Push"))
            {
                moves.Add("Push");
                movesprice.Add(90);
            }
            if (!moveset.Contains("Rolling Thunder"))
            {
                moves.Add("Rolling Thunder");
                movesprice.Add(160);
            }
            if (!moveset.Contains("Double Kick"))
            {
                moves.Add("Double Kick");
                movesprice.Add(300);
            }
            if (!moveset.Contains("Small Heal"))
            {
                moves.Add("Small Heal (Coming Soon!)");
                movesprice.Add(999999);
            }
            if (!moveset.Contains("Attack Boost (COMING SOON!)"))
            {
                moves.Add("Attack Boost (Coming Soon!)");
                movesprice.Add(999999);
            }
        }
    }
}