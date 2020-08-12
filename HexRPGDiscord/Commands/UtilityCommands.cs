using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;

namespace HexRPGDiscord.Commands {
    public class UtilityCommands : BaseCommandModule {
        bool inFight = false;
        string jsonplayer;
        string uname;
        string ustyle;
        Player user;
        [Command ("start")]
        [Description ("Starts a user account.")]
        public async Task Start (CommandContext ctx) {
            if (File.Exists ($@"playersaves/{ctx.User.Id}.json")) {
                Console.WriteLine ($"Player save data found, no need to start!");
            } else {
                var nameEntry = new DiscordEmbedBuilder {
                    Title = "What is your name?",
                    Color = DiscordColor.HotPink
                };

                await ctx.Channel.SendMessageAsync (embed: nameEntry).ConfigureAwait (false);

                var interactivity = ctx.Client.GetInteractivity ();

                var tempusername = await interactivity.WaitForMessageAsync (x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait (false);

                string uname = tempusername.Result.Content.ToString ();

                var styleEntry = new DiscordEmbedBuilder {
                    Title = $"What style will you use, {uname}?",
                    Description = "Mage \nHunter \nRogue",
                    Color = DiscordColor.HotPink
                };

                await ctx.Channel.SendMessageAsync (embed: styleEntry).ConfigureAwait (false);

                while (ustyle == null) // while a style is not assigned, keep checking their query
                {

                    interactivity = ctx.Client.GetInteractivity ();

                    var tempstyle = await interactivity.WaitForMessageAsync (x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait (false);

                    if (tempstyle.Result.Content.ToLower () == "mage") {
                        ustyle = "Mage";
                    } // if (tempstyle.Result.Content.ToLower() == "mage")
                    else if (tempstyle.Result.Content.ToLower () == "hunter") {
                        ustyle = "Hunter";
                    } // else if (tempstyle.Result.Content.ToLower() == "hunter")
                    else if (tempstyle.Result.Content.ToLower () == "rogue") {
                        ustyle = "Rogue";
                    } // else if (tempstyle.Result.Content.ToLower() == "rogue")
                    else if (tempstyle.Result.Content.ToLower () == "admin") {
                        ustyle = "Admin";
                    } // else if (tempstyle.Result.Content.ToLower() == "admin")
                } // while (ustyle == null)

                var usercreatedEmbed = new DiscordEmbedBuilder {
                    Title = "User account created!",
                    Color = DiscordColor.Green
                };

                var usercreatedmessage = await ctx.Channel.SendMessageAsync (embed: usercreatedEmbed).ConfigureAwait (false);

                user = new Player (uname, ustyle, ctx.User.Id.ToString ());

                save ();

                var userinfoEmbed = new DiscordEmbedBuilder {
                    Title = $"{user.name}'s stats:",
                    Description = $"Style: {user.style}\n" +
                    $"Level: {user.level}\n" +
                    $"XP: {user.currentXp}/{user.targetXp}\n" +
                    $"Health: {user.health}\n" +
                    $"Attack: {user.attack}\n" +
                    $"Speed: {user.speed}\n" +
                    $"Moveset: {user.moveset[0]}, {user.moveset[1]}, {user.moveset[2]}, {user.moveset[3]}, {user.moveset[4]}\n" +
                    $"Balance: ${user.balance}\n" +
                    $"Critical Hit Chance: {user.critchance}%\n" +
                    $"Attack Evasion Chance: {user.evaschance}%",
                    Color = DiscordColor.MidnightBlue
                };

                await ctx.Channel.SendMessageAsync (embed: userinfoEmbed).ConfigureAwait (false);
            } // if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
        }

        [Command ("play")]
        [Description ("Starts a game.")]
        public async Task Play (CommandContext ctx) {
            if (File.Exists ($@"playersaves/{ctx.User.Id}.json")) {
                jsonplayer = File.ReadAllText ($@"playersaves/{ctx.User.Id}.json");
                user = JsonConvert.DeserializeObject<Player> (jsonplayer);
                Console.WriteLine ($"Found player data for {user.name}");

                var gameEmbed = new DiscordEmbedBuilder {
                    Title = "Creating game...",
                    Color = DiscordColor.Orange
                };

                await ctx.Channel.SendMessageAsync (embed: gameEmbed).ConfigureAwait (false);

                await startgame (ctx);

            } else {
                Console.WriteLine ($"No player data found! '!start' must be used.");
            } // if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
        }

        // string[] profileAliases = { "profile", "id", "card", "player", "info", "level" };
        [Command ("profile")]
        [Description ("Displays the user's profile.")]
        public async Task Profile (CommandContext ctx) {
            if (File.Exists ($@"playersaves/{ctx.User.Id}.json")) {
                jsonplayer = File.ReadAllText ($@"playersaves/{ctx.User.Id}.json");
                user = JsonConvert.DeserializeObject<Player> (jsonplayer);
                Console.WriteLine ($"Found player data for {user.name}");

                var userinfoEmbed = new DiscordEmbedBuilder {
                    Title = $"{user.name}'s stats:",
                    Description = $"Style: {user.style}\n" +
                    $"Level: {user.level}\n" +
                    $"XP: {user.currentXp}/{user.targetXp}\n" +
                    $"Health: {user.health}\n" +
                    $"Attack: {user.attack}\n" +
                    $"Speed: {user.speed}\n" +
                    $"Moveset: {user.moveset[0]}, {user.moveset[1]}, {user.moveset[2]}, {user.moveset[3]}, {user.moveset[4]}\n" +
                    $"Balance: ${user.balance}\n" +
                    $"Critical Hit Chance: {user.critchance}%\n" +
                    $"Attack Evasion Chance: {user.evaschance}%",
                    Color = DiscordColor.MidnightBlue
                };

                await ctx.Channel.SendMessageAsync (embed: userinfoEmbed).ConfigureAwait (false);
            } // if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
        }

        [Command ("clear")]
        [Description ("Clears all player data associated with the user's account.")]
        public async Task ClearData (CommandContext ctx) {
            if (File.Exists ($@"playersaves/{ctx.User.Id}.json")) {
                var confirmClearEmbed = new DiscordEmbedBuilder {
                    Title = "Are you sure you would like to **clear** your player data? This is a **permanent** action.",
                    Description = "yes / no",
                    Color = DiscordColor.Red
                };

                await ctx.Channel.SendMessageAsync (embed: confirmClearEmbed).ConfigureAwait (false);

                var interactivity = ctx.Client.GetInteractivity ();

                var clearconfirmationResponse = await interactivity.WaitForMessageAsync (x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait (false);

                var dataclearedEmbed = new DiscordEmbedBuilder {
                    Title = "[data_clear_status]",
                    Color = DiscordColor.Red
                };

                if (clearconfirmationResponse.Result.Content.ToLower () == "yes") {
                    dataclearedEmbed.Title = "Player data was cleared.";
                    File.Delete ($@"playersaves/{ctx.User.Id}.json");
                } else {
                    dataclearedEmbed.Title = "Player data was not cleared.";
                    dataclearedEmbed.Color = DiscordColor.Green;
                } // if (clearconfirmationResponse.Result.Content.ToLower() == "yes")

                await ctx.Channel.SendMessageAsync (embed: dataclearedEmbed).ConfigureAwait (false);
            } else {
                var invalidClearEmbed = new DiscordEmbedBuilder {
                    Title = "There is not any player data to clear.",
                    Color = DiscordColor.Red
                };

                await ctx.Channel.SendMessageAsync (embed: invalidClearEmbed).ConfigureAwait (false);
            } // if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
        }

        [Command ("stopgame")]
        [Description ("Stops games")]
        [RequireRoles (RoleCheckMode.Any, "Tester")]
        public async Task StopGame (CommandContext ctx) {
            inFight = false;
        }

        [Command ("shop")]
        [Description ("Shows the user the shop")]
        public async Task Shop (CommandContext ctx) {
            if (File.Exists ($@"playersaves/{ctx.User.Id}.json")) {
                jsonplayer = File.ReadAllText ($@"playersaves/{ctx.User.Id}.json");
                user = JsonConvert.DeserializeObject<Player> (jsonplayer);
                Console.WriteLine ($"Found player data for {user.name}");

                Shop playershop = new Shop (user.level, user.moveset);

                var shopEmbed = new DiscordEmbedBuilder {
                    Title = $"Welcome to the shop {user.name}!",
                    Description = $"Your Balance: ${user.balance}"
                };

                shopEmbed.AddField ($"{playershop.moves[0]}", $"${playershop.movesprice[0]}", true);

                shopEmbed.AddField ($"{playershop.moves[1]}", $"${playershop.movesprice[1]}", true);

                shopEmbed.AddField ($"{playershop.moves[2]}", $"${playershop.movesprice[2]}", true);

                shopEmbed.AddField ($"{playershop.moves[3]}", $"${playershop.movesprice[3]}", true);

                await ctx.Channel.SendMessageAsync (embed: shopEmbed).ConfigureAwait (false);
            } // if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
            else {
                Console.WriteLine ($"No player data found! '!start' must be used.");
            } // if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
        } // public async Task Shop(CommandContext ctx)

        void save () {
            string tempuserinfo = JsonConvert.SerializeObject (user);
            File.WriteAllText ($@"playersaves/{user.Id}.json", tempuserinfo);
        }

        public async Task startgame (CommandContext ctx) {
            Console.WriteLine ($"{user.name} is in a {user.level} game.");

            Enemy enemy = new Enemy (user.level); // creates an enemy depending on the user's level

            var enemyEmbed = new DiscordEmbedBuilder {
                Title = "An enemy appeared!",
                Description = $"Name: {enemy.name}\n" +
                $"Appearance: {enemy.app}\n" +
                $"Health: {enemy.health}\n" +
                $"Attack: {enemy.attack}\n" +
                $"Speed: {enemy.speed}\n",
                Color = DiscordColor.DarkRed
            };

            int phealth = user.health;
            int eohealth = enemy.health;
            int damage = 0;
            int moveint = 0;
            string move;
            inFight = true;
            int xpGained = 0;
            int moneyGained = 0;
            Random rnd = new Random ();

            System.Threading.Thread.Sleep (3000);

            await ctx.Channel.SendMessageAsync (embed: enemyEmbed).ConfigureAwait (false);

            var movesetEmbed = new DiscordEmbedBuilder {
                Title = "What will you do?\nYour moves:",
                Description = $"1. {user.moveset[0]}\n" +
                $"2. {user.moveset[1]}\n" +
                $"3. {user.moveset[2]}\n" +
                $"4. {user.moveset[3]}\n" +
                $"5. {user.moveset[4]}\n" +
                $"\nEnter the number of a move to use it.",
                Color = DiscordColor.Aquamarine
            };

            while (inFight) {
                System.Threading.Thread.Sleep (500);

                await ctx.Channel.SendMessageAsync (embed: movesetEmbed).ConfigureAwait (false);

                var interactivity = ctx.Client.GetInteractivity ();

                var moveinput = await interactivity.WaitForMessageAsync (x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait (false);

                moveint = System.Convert.ToInt32 (moveinput.Result.Content);
                move = user.moveset[moveint - 1];

                var damageEmbed = new DiscordEmbedBuilder {
                    Title = $"[damage info here]",
                    Color = DiscordColor.Azure
                };

                if (user.basemoves.Contains (move)) {
                    damage = System.Convert.ToInt32 (user.attack * 0.15);
                    int critprob = rnd.Next (1, 101);

                    if (critprob <= user.critchance) // if it is a critical hit
                    {
                        damage = System.Convert.ToInt32 (damage * 1.5);
                    } // if (critprob <= user.critchance)
                    enemy.health -= damage;
                } // if (user.basemoves.Contains(move))
                if (enemy.health > 0) // if the enemy still has health
                {
                    damageEmbed.Title = $"Your {move} did {damage} damage!\n" +
                        $"The {enemy.name} now has {enemy.health}/{eohealth} health.";

                    System.Threading.Thread.Sleep (1000);

                    await ctx.Channel.SendMessageAsync (embed: damageEmbed).ConfigureAwait (false);

                    string enemymove = enemy.moveset[user.level - 1, rnd.Next (0, 2)];

                    switch (enemymove) {
                        case "Scratch":
                            phealth -= enemy.attack / 5;
                            damageEmbed.Title = $"The {enemy.name} used {enemymove}, " +
                                $"dealing {enemy.attack / 5} damage!\n" +
                                $"You now have {phealth} health.";
                            break;
                        case "Small Heal":
                            int healthchange = eohealth / 5;
                            enemy.health += healthchange;
                            damageEmbed.Title = $"The {enemy.name} used {enemymove}, " +
                                $"healing by {healthchange}.\n" +
                                $"They now have {enemy.health} health.";
                            break;
                        case "Punch":
                            phealth -= enemy.attack / 4;
                            damageEmbed.Title = $"The {enemy.name} used {enemymove}, " +
                                $"dealing {enemy.attack / 4} damage!\n" +
                                $"You now have {phealth} health.";
                            break;
                    }
                    System.Threading.Thread.Sleep (1000);
                    await ctx.Channel.SendMessageAsync (embed: damageEmbed).ConfigureAwait (false);
                } // if (enemy.health > 0)
                else {
                    damageEmbed.Title = $"Your {move} did {damage} damage, " +
                        $"defeating the {enemy.app} {enemy.name}!";
                    System.Threading.Thread.Sleep (1000);
                    await ctx.Channel.SendMessageAsync (embed: damageEmbed).ConfigureAwait (false);
                    moneyGained = (eohealth / 10) * 3;
                    user.balance += moneyGained;
                    xpGained = eohealth / 3;
                    await addXp (xpGained, moneyGained, ctx);

                }
                if (phealth <= 0) {
                    damageEmbed.Title = $"Your {move} did {damage} damage, " +
                        $"defeating the {enemy.app} {enemy.name}!";
                    System.Threading.Thread.Sleep (1000);
                    await ctx.Channel.SendMessageAsync (embed: damageEmbed).ConfigureAwait (false);
                }
                if (enemy.health <= 0 || phealth <= 0) {
                    inFight = false;
                } // if (enemy.health <= 0 || phealth <= 0)

            } // while (inFight)

            save ();

        } // public async Task startgame(CommandContext ctx)

        public async Task addXp (int amount, int moneyGained, CommandContext ctx) {
            user.currentXp += amount;
            var winEmbed = new DiscordEmbedBuilder {
                Title = $"Congratulations! You gained {amount} XP and ${moneyGained}! You now have ${user.balance}.",
                Color = DiscordColor.Yellow
            };

            await ctx.Channel.SendMessageAsync (embed: winEmbed).ConfigureAwait (false);
            if (user.currentXp >= user.targetXp) {
                user.currentXp -= user.targetXp;
                user.level++;
                user.targetXp += user.targetXp / 10; // increments target by 10% of the previous level's target
                var levelupEmbed = new DiscordEmbedBuilder {
                    Title = $"LEVEL UP!\n" +
                    $"You reached {user.level}!",
                    Color = DiscordColor.Purple
                };

                await ctx.Channel.SendMessageAsync (embed: levelupEmbed).ConfigureAwait (false);

            } // if (user.currentXp >= user.targetXp)

        } // public async Task addXp(int amount, CommandContext ctx)

    } // public class UtilityCommands : BaseCommandModule

} // namespace HexRPGDiscord.Commands