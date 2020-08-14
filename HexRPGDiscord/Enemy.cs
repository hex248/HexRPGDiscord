using System;
using System.Collections.Generic;
using System.Text;

namespace HexRPGDiscord
{
    class Enemy
    {
        public string name;
        public string app; // app = Appearance
        public int health;
        public int attack;
        public int speed;
        public string[,] enemynames =
        {
            { "Gremlin", "Goblin"},
            { "Orc", "Griffin" },
            { "Phoenix", "Elemental" }
        };
        public string[,] enemyapps =
        {
            { "Red, small", "Blue, ambidextrous" },
            { "Red, moderately large", "Blue, wise" },
            { "Orange, immense", "Green, six-eyed" }
        };
        public string[,] moveset =
        {
            { "Scratch", "Small Heal" },
            { "Punch", "Small Heal" },
            { "Firebolt", "Hidden Attack" }
        };
        public Enemy(int plevel)
        {
            Random rnd = new Random();
            if (plevel > 2)
            {
                name = enemynames[rnd.Next(0, 2), rnd.Next(0, 2)];
                app = enemyapps[rnd.Next(0, 2), rnd.Next(0, 2)];
            }
            else
            {
                name = enemynames[plevel - 1, rnd.Next(0, 2)];
                app = enemyapps[plevel - 1, rnd.Next(0, 2)];
            }

            switch (name)
            {
                case "Gremlin":
                    health = 60;
                    attack = 70;
                    speed = 50;
                    return;
                case "Goblin":
                    health = 70;
                    attack = 60;
                    speed = 80;
                    return;
                case "Orc":
                    health = 120;
                    attack = 90;
                    speed = 20;
                    return;
                case "Griffin":
                    health = 60;
                    attack = 60;
                    speed = 100;
                    return;
                case "Phoenix":
                    health = 110;
                    attack = 100;
                    speed = 80;
                    break;
                case "Elemental":
                    health = 100;
                    attack = 130;
                    speed = 110;
                    break;
            }
        }
    }
}
