using System;
using System.Collections.Generic;
using System.Text;

namespace HexRPGDiscord
{
    class Player
    {
        public string name;
        public string style;
        public string Id;
        public int level;
        public int targetXp;
        public int currentXp;
        public int health;
        public int attack;
        public int speed;
        public string[] moveset = new string[5];
        public string[] basemoves = { "Firebolt", "Piercing Arrow", "Punch" };
        public string[] tier2moves = { "Kick" };
        public string[] tier3moves = { "Push" };
        public string[] tier4moves; // = { "Attack Boost" };
        public string[] tier5moves; // = { "Small Heal" };
        public int balance;
        public int critchance;
        public int evaschance;

        public Player(string pname, string pstyle, string pId)
        {
            name = pname;
            style = pstyle;
            Id = pId;
            switch (pstyle)
            {
                case "Mage":
                    level = 1;
                    targetXp = 50;
                    currentXp = 0;
                    health = 90;
                    attack = 140;
                    speed = 80;
                    moveset[0] = "Firebolt";
                    moveset[1] = "None";
                    moveset[2] = "None";
                    moveset[3] = "None";
                    moveset[4] = "Run";
                    balance = 0;
                    critchance = 10;
                    evaschance = 10;
                    break;

                case "Hunter":
                    level = 1;
                    targetXp = 50;
                    currentXp = 0;
                    health = 130;
                    attack = 120;
                    speed = 110;
                    moveset[0] = "Piercing Arrow";
                    moveset[1] = "None";
                    moveset[2] = "None";
                    moveset[3] = "None";
                    moveset[4] = "Run";
                    balance = 0;
                    critchance = 20;
                    evaschance = 10;
                    break;

                case "Rogue":
                    level = 1;
                    targetXp = 50;
                    currentXp = 0;
                    health = 60;
                    attack = 145;
                    speed = 150;
                    moveset[0] = "Punch";
                    moveset[1] = "None";
                    moveset[2] = "None";
                    moveset[3] = "None";
                    moveset[4] = "Run";
                    balance = 0;
                    critchance = 35;
                    evaschance = 20;
                    break;

                case "Admin":
                    level = 1;
                    targetXp = 50;
                    health = 1000000;
                    attack = 1000000;
                    speed = 1000000;
                    moveset[0] = "Punch";
                    moveset[1] = "None";
                    moveset[2] = "None";
                    moveset[3] = "None";
                    moveset[4] = "Fly";
                    balance = 1000000;
                    critchance = 100;
                    evaschance = 100;
                    break;
            }
        }
    }
}
