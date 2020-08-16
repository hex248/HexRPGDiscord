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
            { "Gremlin", "Goblin"}, // LVL1
            { "Gremlin", "Goblin"}, // LVL2
            { "Gremlin", "Goblin"}, // LVL3
            { "Wolf", "Murlock" }, // LVL4
            { "Wolf", "Murlock" }, // LVL5
            { "Kobald", "Troll" }, // LVL6
            { "Kobald", "Troll" }, // LVL7
            { "Kobald", "Troll" }, // LVL8
            { "Minatour", "Nue" }, // LVL9
            { "Minatour", "Nue" }, // LVL10
            { "Minatour", "Nue" }, // LVL11
            { "Minatour", "Nue" }, // LVL12
            { "Hieracosphinx", "Orc" }, // LVL13
            { "Hieracosphinx", "Orc" }, // LVL14
            { "Hieracosphinx", "Orc" }, // LVL15
            { "Hieracosphinx", "Orc" }, // LVL16
            { "Wyvern", "Golem" }, // LVL17
            { "Wyvern", "Golem" }, // LVL18
            { "Wyvern", "Golem" }, // LVL19
            { "Wyvern", "Golem" }, // LVL20
            { "Wyvern", "Golem" }, // LVL21
            { "Giant", "Griffin" }, // LVL22
            { "Giant", "Griffin" }, // LVL23
            { "Giant", "Griffin" }, // LVL24
            { "Giant", "Griffin" }, // LVL25
            { "Elemental", "Spirit" }, // LVL26
            { "Elemental", "Spirit" }, // LVL27
            { "Elemental", "Spirit" }, // LVL28
            { "Elemental", "Spirit" }, // LVL29
            { "Dragon", "Phoenix" }, // LVL30
            { "Dragon", "Phoenix" }, // LVL31
            { "Dragon", "Phoenix" }, // LVL32
            { "Dragon", "Phoenix" }, // LVL33
            { "Dragon", "Phoenix" }, // LVL34
            { "Dragon", "Phoenix" }, // LVL35
            { "Dragon", "Phoenix" }, // LVL36
            { "Dragon", "Phoenix" } // LVL37
        };
        public string[,] enemyapps =
        {
            { "Red, small", "Blue, ambidextrous" }, // LVL1
            { "Red, small", "Blue, ambidextrous" }, // LVL2
            { "Red, small", "Blue, ambidextrous" }, // LVL3
            { "Grey, furry", "Red, buff" }, // LVL4
            { "Grey, furry", "Red, buff" }, // LVL5
            { "Murky-green, ugly", "Small, vile" }, // LVL6
            { "Murky-green, ugly", "Small, vile" }, // LVL7
            { "Murky-green, ugly", "Small, vile" }, // LVL8
            { "Black, fuwa fuwa", "Green, vicious" }, // LVL9
            { "Black, fuwa fuwa", "Green, vicious" }, // LVL10
            { "Black, fuwa fuwa", "Green, vicious" }, // LVL11
            { "Black, fuwa fuwa", "Green, vicious" }, // LVL12
            { "Old, wise", "Young, Dumb" }, // LVL13
            { "Old, wise", "Young, Dumb" }, // LVL14
            { "Old, wise", "Young, Dumb" }, // LVL15
            { "Old, wise", "Young, Dumb" }, // LVL16
            { "Scaly, old", "Earthy, young" }, // LVL17
            { "Scaly, old", "Earthy, young" }, // LVL18
            { "Scaly, old", "Earthy, young" }, // LVL19
            { "Scaly, old", "Earthy, young" }, // LVL20
            { "Scaly, old", "Earthy, young" }, // LVL21
            { "Big, dumb", "Grey, proud" }, // LVL22
            { "Big, dumb", "Grey, proud" }, // LVL23
            { "Big, dumb", "Grey, proud" }, // LVL24
            { "Big, dumb", "Grey, proud" }, // LVL25
            { "Ghostly", "Green" }, // LVL26
            { "Ghostly", "Green" }, // LVL27
            { "Ghostly", "Green" }, // LVL28
            { "Ghostly", "Green" }, // LVL29
            { "Big, Strong, Powerfull, Deadly", "Weak, FUWA FUWA"}, // LVL30
            { "Big, Strong, Powerfull, Deadly", "Weak, FUWA FUWA"}, // LVL31
            { "Big, Strong, Powerfull, Deadly", "Weak, FUWA FUWA"}, // LVL32
            { "Big, Strong, Powerfull, Deadly", "Weak, FUWA FUWA"}, // LVL33
            { "Big, Strong, Powerfull, Deadly", "Weak, FUWA FUWA"}, // LVL34
            { "Big, Strong, Powerfull, Deadly", "Weak, FUWA FUWA"}, // LVL35
            { "Big, Strong, Powerfull, Deadly", "Weak, FUWA FUWA"}, // LVL36
            { "Big, Strong, Powerfull, Deadly", "Weak, FUWA FUWA"} // LVL37
        };

        public string[,] moveset =
        {
            { "Scratch", "Small Heal" }, // LVL1
            { "Scratch", "Small Heal" }, // LVL2
            { "Scratch", "Small Heal" }, // LVL3
            { "Bite", "Scratch" }, // LVL4
            { "Bite", "Scratch" }, // LVL5
            { "Slam", "Pick up" }, // LVL6
            { "Slam", "Pick up" }, // LVL7
            { "Slam", "Pick up" }, // LVL8
            { "Charge", "Bite" }, // LVL9
            { "Charge", "Bite" }, // LVL10
            { "Charge", "Bite" }, // LVL11
            { "Charge", "Bite" }, // LVL12
            { "Chomp", "Stomp" }, // LVL13
            { "Chomp", "Stomp" }, // LVL14
            { "Chomp", "Stomp" }, // LVL15
            { "Chomp", "Stomp" }, // LVL16
            { "Earthquake", "Rock Throw" }, // LVL17
            { "Earthquake", "Rock Throw" }, // LVL18
            { "Earthquake", "Rock Throw" }, // LVL19
            { "Earthquake", "Rock Throw" }, // LVL20
            { "Earthquake", "Rock Throw" }, // LVL21
            { "Jump", "Slam" }, // LVL22
            { "Jump", "Slam" }, // LVL23
            { "Jump", "Slam" }, // LVL24
            { "Jump", "Slam" }, // LVL25
            { "FireBall", "Wind Slash" }, // LVL26
            { "FireBall", "Wind Slash" }, // LVL27
            { "FireBall", "Wind Slash" }, // LVL28
            { "FireBall", "Wind Slash" }, // LVL29
            { "Fire Breath", "Gust" }, // LVL30
            { "Fire Breath", "Gust" }, // LVL31
            { "Fire Breath", "Gust" }, // LVL32
            { "Fire Breath", "Gust" }, // LVL33
            { "Fire Breath", "Gust" }, // LVL34
            { "Fire Breath", "Gust" }, // LVL35
            { "Fire Breath", "Gust" }, // LVL36

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
                case "Gremlin":
                    health = 60;
                    attack = 60;
                    speed = 50;
                    break;
                case "Goblin":
                    health = 70;
                    attack = 60;
                    speed = 80;
                    break;
                case "Wolf":
                    health = 80;
                    attack = 80;
                    speed = 90;
                    break;
                case "Murlock":
                    health = 75;
                    attack = 80;
                    speed = 70;
                    break;
                case "Troll":
                    health = 110;
                    attack = 100;
                    speed = 5;
                    break;
                case "Kobald":
                    health = 60;
                    attack = 110;
                    speed = 70;
                    break;
                case "Minatour":
                    health = 140;
                    attack = 100;
                    speed = 95;
                    break;
                case "Nue":
                    health = 100;
                    attack = 140;
                    speed = 90;
                    break;
                case "Hieracosphinx":
                    health = 190;
                    attack = 130;
                    speed = 70;
                    break;
                case "Orc":
                    health = 200;
                    attack = 120;
                    speed = 10;
                    break;
                case "Wyvern":
                    health = 220;
                    attack = 170;
                    speed = 100;
                    break;
                case "Golem":
                    health = 240;
                    attack = 200;
                    speed = 10;
                    break;
                case "Giant":
                    health = 240;
                    attack = 240;
                    speed = 10;
                    break;
                case "Griffin":
                    health = 200;
                    attack = 220;
                    speed = 75;
                    break;
                case "Elemental":
                    health = 100;
                    attack = 400;
                    speed = 80;
                    break;
                case "Spirit":
                    health = 400;
                    attack = 100;
                    speed = 80;
                    break;
                case "Dragon":
                    health = 300;
                    attack = 150;
                    speed = 90;
                    break;
                case "Phoenix":
                    health = 350;
                    attack = 200;
                    speed = 50;
                    break;

            }
        }
    }
}