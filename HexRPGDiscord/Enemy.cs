using System;
using System.Collections.Generic;
using System.Text;

namespace HexRPGDiscord
{
    class Enemy
    {
        public string name; // This is randomly chosen from a selection which changes depending on the user's level
        public string app; // app = Appearance
        public int health; // This is defined depending on the name of the enemy
        public int attack; // This is defined depending on the name of the enemy
        public int speed; // This is defined depending on the name of the enemy
        public string[,] enemynames =
        {
            { "Gremlin", "Goblin"}, // Level 1
            { "Orc", "Griffin" }, // Level 2
            { "Phoenix", "Elemental" } // Level 3
        };
        public string[,] enemyapps =
        {
            { "Red, small", "Blue, ambidextrous" }, // Level 1
            { "Red, moderately large", "Blue, wise" }, // Level 2
            { "Orange, immense", "Green, six-eyed" } // Level 3
        };
        public string[,] moveset =
        {
            { "Scratch", "Small Heal" }, // Level 1
            { "Punch", "Small Heal" }, // Level 2
            { "Firebolt", "Hidden Attack" } // Level 3
        };
        public Enemy(int plevel)
        {
            Random rnd = new Random();
            if (plevel > enemynames.Length / 2 && plevel < enemynames.Length)
            {
                name = enemynames[rnd.Next(plevel - 3, 3), rnd.Next(0, 2)];
                app = enemyapps[rnd.Next(plevel - 3, 3), rnd.Next(0, 2)];
            }
            else if (plevel >= enemynames.Length)
            {
                name = enemynames[2, rnd.Next(0, 2)];
                app = enemyapps[2, rnd.Next(0, 2)];
            }
            else
            {
                name = enemynames[plevel - 1, rnd.Next(0, 2)];
                app = enemyapps[plevel - 1, rnd.Next(0, 2)];
            }

            switch (name)
            {
                // Level 1

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

                // Level 2

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

                // Level 3

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