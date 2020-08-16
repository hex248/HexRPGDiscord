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
        public int skillpoints;
        public int ohealth;
        public int extrahealth = 0;
        public int health;
        public int oattack;
        public int extraattack = 0;
        public int attack;
        public int speed;
        public string[] moveset = new string[4];
        public string[] basemoves = { "Firebolt", "Piercing Arrow", "Punch" };
        public string[] tier2moves = { "Kick" };
        public string[] tier3moves = { "Push" };
        public string[] tier4moves = { "Rolling Thunder" };
        public string[] tier5moves = { "Double Kick" };
        public string lastDailyDate;
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
                    ohealth = 90;
                    oattack = 140;
                    speed = 80;
                    moveset[0] = "Firebolt";
                    moveset[1] = "None";
                    moveset[2] = "None";
                    moveset[3] = "None";
                    balance = 0;
                    critchance = 10;
                    evaschance = 10;
                    break;

                case "Hunter":
                    level = 1;
                    targetXp = 50;
                    currentXp = 0;
                    ohealth = 130;
                    oattack = 120;
                    speed = 110;
                    moveset[0] = "Piercing Arrow";
                    moveset[1] = "None";
                    moveset[2] = "None";
                    moveset[3] = "None";
                    balance = 0;
                    critchance = 20;
                    evaschance = 10;
                    break;

                case "Rogue":
                    level = 1;
                    targetXp = 50;
                    currentXp = 0;
                    ohealth = 60;
                    oattack = 145;
                    speed = 150;
                    moveset[0] = "Punch";
                    moveset[1] = "None";
                    moveset[2] = "None";
                    moveset[3] = "None";
                    balance = 0;
                    critchance = 35;
                    evaschance = 20;
                    break;

                case "Admin":
                    level = 1;
                    targetXp = 50;
                    currentXp = 0;
                    ohealth = 1000000;
                    oattack = 1000000;
                    speed = 1000000;
                    moveset[0] = "Punch";
                    moveset[1] = "None";
                    moveset[2] = "None";
                    moveset[3] = "None";
                    balance = 1000000;
                    critchance = 100;
                    evaschance = 100;
                    break;
            }
        }
    }
}
