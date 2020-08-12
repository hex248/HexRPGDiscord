using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexRPGDiscord
{
    class Shop
    {
        public string[] moves = new string[5];
        public int[] movesprice = new int[5];

        public Shop(int ulevel, string[] moveset)
        {
            switch (ulevel)
            {
                case 1: // Level 1 Shop
                    if (!moveset.Contains("Kick"))
                    {
                        moves[0] = "Kick";
                        movesprice[0] = 10;
                    }
                    else
                    {
                        moves[0] = "Kick (Already using)";
                    }
                    if (!moveset.Contains("Push"))
                    {
                        moves[1] = "Push";
                        movesprice[1] = 10;
                    }
                    else
                    {
                        moves[1] = "Push (Already using)";
                    }
                    if (!moveset.Contains("Small Heal"))
                    {
                        moves[2] = "Small Heal";
                        movesprice[2] = 10;
                    }
                    else
                    {
                        moves[2] = "Small Heal (Already using)";
                    }
                    if (!moveset.Contains("Attack Boost"))
                    {
                        moves[3] = "Attack Boost";
                        movesprice[3] = 10;
                    }
                    else
                    {
                        moves[3] = "Attack Boost (Already using)";
                    }
                    break;
            }
        }
    }
}
