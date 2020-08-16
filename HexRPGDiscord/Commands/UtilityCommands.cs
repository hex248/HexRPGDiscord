using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;

namespace HexRPGDiscord.Commands
{
    public class UtilityCommands : BaseCommandModule
    {
        private bool inFight = false;
        private string jsonplayer;
        private string ustyle;
        private Player user;

        [Command("start")]
        [Description("Starts a user account.")]
        public async Task Start(CommandContext ctx)
        {
            if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
            {
                var DataFoundEmbed = new DiscordEmbedBuilder
                {
                    Title = "You have already created a character! Use !play to start playing."
                };

                await ctx.Channel.SendMessageAsync(embed: DataFoundEmbed).ConfigureAwait(false);
            }
            else
            {
                var nameEntry = new DiscordEmbedBuilder
                {
                    Title = "What is your name?",
                    Color = DiscordColor.HotPink
                };

                await ctx.Channel.SendMessageAsync(embed: nameEntry).ConfigureAwait(false);

                var interactivity = ctx.Client.GetInteractivity();

                var tempusername = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);

                string uname = tempusername.Result.Content.ToString();

                var styleEntry = new DiscordEmbedBuilder
                {
                    Title = $"What style will you use, {uname}?",
                    Description = "Mage \nHunter \nRogue",
                    Color = DiscordColor.HotPink
                };

                await ctx.Channel.SendMessageAsync(embed: styleEntry).ConfigureAwait(false);

                while (ustyle == null) // while a style is not assigned, keep checking their query
                {
                    interactivity = ctx.Client.GetInteractivity();

                    var tempstyle = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);

                    if (tempstyle.Result.Content.ToLower() == "mage")
                    {
                        ustyle = "Mage";
                    } // if (tempstyle.Result.Content.ToLower() == "mage")
                    else if (tempstyle.Result.Content.ToLower() == "hunter")
                    {
                        ustyle = "Hunter";
                    } // else if (tempstyle.Result.Content.ToLower() == "hunter")
                    else if (tempstyle.Result.Content.ToLower() == "rogue")
                    {
                        ustyle = "Rogue";
                    } // else if (tempstyle.Result.Content.ToLower() == "rogue")
                    else if (tempstyle.Result.Content.ToLower() == "admin")
                    {
                        ustyle = "Admin";
                    } // else if (tempstyle.Result.Content.ToLower() == "admin")
                } // while (ustyle == null)

                var usercreatedEmbed = new DiscordEmbedBuilder
                {
                    Title = "User account created!",
                    Color = DiscordColor.Green
                };

                var usercreatedmessage = await ctx.Channel.SendMessageAsync(embed: usercreatedEmbed).ConfigureAwait(false);

                user = new Player(uname, ustyle, ctx.User.Id.ToString());

                ustyle = null;

                await Task.Run(() => save(ctx));

                var userinfoEmbed = new DiscordEmbedBuilder
                {
                    Title =
                    $"{user.name}'s stats:",
                    Description =
                    $"Style: {user.style}\n" +
                    $"Level: {user.level}\n" +
                    $"XP: {user.currentXp}/{user.targetXp}\n" +
                    $"Health: {user.health}\n" +
                    $"Attack: {user.attack}\n" +
                    $"Speed: {user.speed}\n" +
                    $"Moveset: {user.moveset[0]}, {user.moveset[1]}, {user.moveset[2]}, {user.moveset[3]}\n" +
                    $"Balance: ${user.balance}\n" +
                    $"Critical Hit Chance: {user.critchance}%\n" +
                    $"Attack Evasion Chance: {user.evaschance}%",
                    Color = DiscordColor.MidnightBlue
                };

                userinfoEmbed.WithThumbnail(ctx.User.GetAvatarUrl((DSharpPlus.ImageFormat)5, 1024));

                await ctx.Channel.SendMessageAsync(embed: userinfoEmbed).ConfigureAwait(false);
            } // if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
        }

        [Command("play")]
        [Description("Starts a game.")]
        public async Task Play(CommandContext ctx)
        {
            if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
            {
                await Task.Run(() => LoadUser(ctx.User.Id.ToString()));

                var gameEmbed = new DiscordEmbedBuilder
                {
                    Title = "Creating game...",
                    Color = DiscordColor.Orange
                };

                await ctx.Channel.SendMessageAsync(embed: gameEmbed).ConfigureAwait(false);

                await startgame(ctx);
            }
            else
            {
                var noDataEmbed = new DiscordEmbedBuilder
                {
                    Title = "You have not created a character yet! Use !start to get started on your adventure."
                };

                await ctx.Channel.SendMessageAsync(embed: noDataEmbed).ConfigureAwait(false);
            } // if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
        }

        // string[] profileAliases = { "profile", "id", "card", "player", "info", "level" };
        [Command("profile")]
        [Description("Displays the user's profile.")]
        public async Task Profile(CommandContext ctx)
        {
            if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
            {
                await Task.Run(() => LoadUser(ctx.User.Id.ToString()));

                var userinfoEmbed = new DiscordEmbedBuilder
                {
                    Title =
                    $"{user.name}'s stats:",
                    Description =
                    $"Style: {user.style}\n" +
                    $"Level: {user.level}\n" +
                    $"XP: {user.currentXp}/{user.targetXp}\n" +
                    $"Health: {user.health}\n" +
                    $"Attack: {user.attack}\n" +
                    $"Speed: {user.speed}\n" +
                    $"Moveset: {user.moveset[0]}, {user.moveset[1]}, {user.moveset[2]}, {user.moveset[3]}\n" +
                    $"Balance: ${user.balance}\n" +
                    $"Critical Hit Chance: {user.critchance}%\n" +
                    $"Attack Evasion Chance: {user.evaschance}%",
                    Color = DiscordColor.MidnightBlue
                };

                userinfoEmbed.WithThumbnail(ctx.User.GetAvatarUrl((DSharpPlus.ImageFormat)5, 1024));

                await ctx.Channel.SendMessageAsync(embed: userinfoEmbed).ConfigureAwait(false);

            } // if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
            else
            {
                var noDataEmbed = new DiscordEmbedBuilder
                {
                    Title = "You have not created a character yet! Use !start to get started on your adventure."
                };

                await ctx.Channel.SendMessageAsync(embed: noDataEmbed).ConfigureAwait(false);
            }
        }

        [Command("leaderboard")]
        [Description("Displays the leaderboard for the current server.")]
        public async Task Leaderboard(CommandContext ctx)
        {
            List<string> nameleaderboardPlacements = new List<string>();
            List<string> levelleaderboardPlacements = new List<string>();

            string leaderboardDescription = "";


            await Task.Run(() => LoadUser(ctx.User.Id.ToString()));
            await Task.Run(() => save(ctx));


            string leaderboard = File.ReadAllText($@"leaderboards/{ctx.Guild.Id}.json");
            Dictionary<string, int> leaderboardDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(leaderboard);

            foreach (KeyValuePair<string, int> player in leaderboardDict.OrderBy(key => key.Value))
            {
                nameleaderboardPlacements.Insert(0, $"{player.Key}");
                levelleaderboardPlacements.Insert(0, $"{player.Value}");
            }

            for (int i = 0; i < nameleaderboardPlacements.Count; i++)
            {
                leaderboardDescription += $"**{nameleaderboardPlacements[i]}**: {levelleaderboardPlacements[i]}\n";
            }

            var leaderboardEmbed = new DiscordEmbedBuilder
            {
                Title = $"**{ctx.Guild.Name}**'s Leaderboard:",
                Description = leaderboardDescription
            };

            await ctx.Channel.SendMessageAsync(embed: leaderboardEmbed).ConfigureAwait(false);
        }

        [Command("clear")]
        [Description("Clears all player data associated with the user's account.")]
        public async Task ClearData(CommandContext ctx)
        {
            if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
            {
                var confirmClearEmbed = new DiscordEmbedBuilder
                {
                    Title = "Are you sure you would like to **clear** your player data? This is a **permanent** action.",
                    Description = "yes / no",
                    Color = DiscordColor.Red
                };

                await ctx.Channel.SendMessageAsync(embed: confirmClearEmbed).ConfigureAwait(false);

                var interactivity = ctx.Client.GetInteractivity();

                var clearconfirmationResponse = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);

                var dataclearedEmbed = new DiscordEmbedBuilder
                {
                    Title = "[data_clear_status]",
                    Color = DiscordColor.Red
                };

                if (clearconfirmationResponse.Result.Content.ToLower() == "yes")
                {
                    dataclearedEmbed.Title = "Player data was cleared.";
                    File.Delete($@"playersaves/{ctx.User.Id}.json");
                }
                else
                {
                    dataclearedEmbed.Title = "Player data was not cleared.";
                    dataclearedEmbed.Color = DiscordColor.Green;
                } // if (clearconfirmationResponse.Result.Content.ToLower() == "yes")

                await ctx.Channel.SendMessageAsync(embed: dataclearedEmbed).ConfigureAwait(false);
            }
            else
            {
                var invalidClearEmbed = new DiscordEmbedBuilder
                {
                    Title = "There is not any player data to clear.",
                    Color = DiscordColor.Red
                };

                await ctx.Channel.SendMessageAsync(embed: invalidClearEmbed).ConfigureAwait(false);
            } // if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
        }

        [Command("skills")]
        [Description("Shows the skill point allocation menu.")]
        public async Task Skills(CommandContext ctx)
        {
            if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
            {
                await Task.Run(() => LoadUser(ctx.User.Id.ToString()));

                var skillEmbed = new DiscordEmbedBuilder
                {
                    Title = 
                    $"**Skills:**\n\n",
                    Description = 
                    $"Your Skill Points: {user.skillpoints}"
                };

                skillEmbed.AddField("Attack:", $"{user.attack}", false);
                skillEmbed.AddField("Health:", $"{user.health}", false);
                skillEmbed.WithFooter("Type either attack or health to start allocating skill points");

                await ctx.Channel.SendMessageAsync(embed: skillEmbed).ConfigureAwait(false);

                var interactivity = ctx.Client.GetInteractivity();

                var skillResponse = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);

                var allocateEmbed = new DiscordEmbedBuilder
                {
                    Title = "attack/health",
                    Description = $"Your Skill Points: {user.skillpoints}"
                };

                if (skillResponse.Result.Content.ToLower() == "attack" || skillResponse.Result.Content.ToLower() == "health")
                {
                    if (skillResponse.Result.Content.ToLower() == "attack")
                    {
                        allocateEmbed.Title = "Type the number of points that you would like to allocate to your attack power.";
                    }
                    else if (skillResponse.Result.Content.ToLower() == "health")
                    {
                        allocateEmbed.Title = "Type the number of points that you would like to allocate to your health.";
                    }

                    await ctx.Channel.SendMessageAsync(embed: allocateEmbed).ConfigureAwait(false);

                    var interactivitytwo = ctx.Client.GetInteractivity();
                    var allocationResponse = await interactivitytwo.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);
                    int allocationNumber = System.Convert.ToInt32(allocationResponse.Result.Content);

                    if (allocationNumber <= user.skillpoints && allocationNumber > 0)
                    {
                        int allocatedValue = allocationNumber * 10;
                        user.skillpoints -= allocationNumber;
                        string allocation = "None";
                        switch (skillResponse.Result.Content.ToLower())
                        {
                            case "attack":
                                user.extraattack += allocatedValue;
                                allocation = "Attack";
                                break;

                            case "health":
                                user.extrahealth += allocatedValue;
                                allocation = "Health";
                                break;
                        }

                        var allocatedEmbed = new DiscordEmbedBuilder
                        {
                            Title = $"You allocated {allocationNumber} skill points to {allocation}.\nThis increased your {allocation} value by {allocationNumber * 10}."
                        };

                        await ctx.Channel.SendMessageAsync(embed: allocatedEmbed).ConfigureAwait(false);

                        await Task.Run(() => save(ctx));
                    }
                    else
                    {
                        var notallocatedEmbed = new DiscordEmbedBuilder
                        {
                            Title = $"You do not have {allocationNumber} skill points to allocate!"
                        };

                        await ctx.Channel.SendMessageAsync(embed: notallocatedEmbed).ConfigureAwait(false);
                    }

                    
                }
            }
            else
            {
                var noDataEmbed = new DiscordEmbedBuilder
                {
                    Title = "You have not created a character yet! Use !start to get started on your adventure."
                };

                await ctx.Channel.SendMessageAsync(embed: noDataEmbed).ConfigureAwait(false);
            }
        }

        [Command("daily")]
        [Description("Allows the player to claim their daily money reward.")]
        public async Task Daily(CommandContext ctx)
        {
            Random rnd = new Random();

            int reward = rnd.Next(26, 46);

            if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
            {
                await Task.Run(() => LoadUser(ctx.User.Id.ToString()));

                if (user.lastDailyDate != DateTime.UtcNow.ToString("dd-MM-yyyy"))
                {
                    user.balance += reward;

                    var dailyEmbed = new DiscordEmbedBuilder
                    {
                        Title = $"You Claimed your daily reward of ${reward}! you now have ${user.balance}.",
                        Color = DiscordColor.Green
                    };

                    await ctx.Channel.SendMessageAsync(embed: dailyEmbed).ConfigureAwait(false);

                    user.lastDailyDate = DateTime.UtcNow.ToString("dd-MM-yyyy");

                    await Task.Run(() => save(ctx));
                }
                else
                {
                    var dailyEmbed = new DiscordEmbedBuilder
                    {
                        Title = $"You have already claimed your reward today! Come back again tomorrow for a reward.",
                        Color = DiscordColor.IndianRed
                    };

                    await ctx.Channel.SendMessageAsync(embed: dailyEmbed).ConfigureAwait(false);
                }

                
            }
            else
            {
                var noDataEmbed = new DiscordEmbedBuilder
                {
                    Title = "You have not created a character yet! Use !start to get started on your adventure."
                };

                await ctx.Channel.SendMessageAsync(embed: noDataEmbed).ConfigureAwait(false);
            }
            

        }

        [Command("shop")]
        [Description("Shows the user the shop")]
        public async Task Shop(CommandContext ctx)
        {
            if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
            {
                await Task.Run(() => LoadUser(ctx.User.Id.ToString()));
                

                Shop playershop = new Shop(user.moveset);


                var shopEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Welcome to the shop {user.name}!",
                    Description = $"Your Balance: ${user.balance}"
                };

                for (int i = 0; i < playershop.moves.Count; i++)
                {
                    shopEmbed.AddField($"{i + 1}. {playershop.moves[i]}", $"${playershop.movesprice[i]}", false);
                }

                // shopEmbed.AddField($"1. {playershop.moves[0]}", $"${playershop.movesprice[0]}", false);

                // shopEmbed.AddField($"2. {playershop.moves[1]}", $"${playershop.movesprice[1]}", false);

                // shopEmbed.AddField($"3. {playershop.moves[2]}", $"${playershop.movesprice[2]}", false);

                // shopEmbed.AddField($"4. {playershop.moves[3]}", $"${playershop.movesprice[3]}", false);

                shopEmbed.WithFooter("Type the number corresponding to the move you would like to buy.");

                await ctx.Channel.SendMessageAsync(embed: shopEmbed).ConfigureAwait(false);

                bool input_is_valid = false;

                int buyChoice = 0;

                var interactivity = ctx.Client.GetInteractivity();

                var buyResponse = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);

                while (!input_is_valid)
                {
                    interactivity = ctx.Client.GetInteractivity();

                    buyResponse = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);

                    buyChoice = System.Convert.ToInt32(buyResponse.Result.Content);

                    if (buyChoice > 0 && buyChoice <= playershop.moves.Count)
                    {
                        input_is_valid = true;
                    }
                }

                if (input_is_valid)
                {
                    if (user.balance >= playershop.movesprice[buyChoice - 1])
                    {
                        input_is_valid = false;

                        var replaceEmbed = new DiscordEmbedBuilder
                        {
                            Title = "Here are your current moves:",
                            Description = $"1. {user.moveset[0]}\n" +
                                          $"2. {user.moveset[1]}\n" +
                                          $"3. {user.moveset[2]}\n" +
                                          $"4. {user.moveset[3]}\n"
                        };

                        replaceEmbed.WithFooter("Type the number corresponding to the move you would like to replace.");

                        await ctx.Channel.SendMessageAsync(embed: replaceEmbed).ConfigureAwait(false);

                        var replaceinteractivity = ctx.Client.GetInteractivity();

                        var replaceResponse = await replaceinteractivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);

                        int replaceChoice = System.Convert.ToInt32(replaceResponse.Result.Content);

                        if (replaceChoice > 0 && replaceChoice <= user.moveset.Length)
                        {
                            input_is_valid = true;
                        }

                        if (input_is_valid)
                        {
                            var boughtEmbed = new DiscordEmbedBuilder
                            {
                                Title = $"You replaced {user.moveset[replaceChoice - 1]} with {playershop.moves[buyChoice - 1]}, spending ${playershop.movesprice[buyChoice - 1]}!"
                            };

                            user.moveset[replaceChoice - 1] = playershop.moves[buyChoice - 1];
                            user.balance -= playershop.movesprice[buyChoice - 1];

                            await ctx.Channel.SendMessageAsync(embed: boughtEmbed).ConfigureAwait(false);

                            await Task.Run(() => save(ctx));
                        }
                    }
                    else
                    {
                        var tooexpensiveEmbed = new DiscordEmbedBuilder
                        {
                            Title = $"You cannot afford {playershop.moves[buyChoice - 1]}.\nYou need ${playershop.movesprice[buyChoice - 1] - user.balance}"
                        };

                        await ctx.Channel.SendMessageAsync(embed: tooexpensiveEmbed).ConfigureAwait(false);
                    }
                }
            } // if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
            else
            {
                var noDataEmbed = new DiscordEmbedBuilder
                {
                    Title = "You have not created a character yet! Use !start to get started on your adventure."
                };

                await ctx.Channel.SendMessageAsync(embed: noDataEmbed).ConfigureAwait(false);
            } // if (File.Exists($@"playersaves/{ctx.User.Id}.json"))
        } // public async Task Shop(CommandContext ctx)

        public async Task save(CommandContext ctx)
        {
            user.health = user.ohealth + user.extrahealth;
            user.attack = user.oattack + user.extraattack;
            string tempuserinfo = JsonConvert.SerializeObject(user);
            File.WriteAllText($@"playersaves/{user.Id}.json", tempuserinfo);

            if (!File.Exists($@"leaderboards/{ctx.Guild.Id}.json"))
            {
                Dictionary<string, int> leaderboardDict = new Dictionary<string, int>();

                string leaderboardjson = JsonConvert.SerializeObject(leaderboardDict, Formatting.Indented);

                File.WriteAllText($@"leaderboards/{ctx.Guild.Id}.json", leaderboardjson);
            }
            if (File.Exists($@"leaderboards/{ctx.Guild.Id}.json"))
            {
                string strleaderboard = File.ReadAllText($@"leaderboards/{ctx.Guild.Id}.json");
                Dictionary<string, int> leaderboardDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(strleaderboard);


                if (leaderboardDict.ContainsKey($"{user.name}"))
                {
                    leaderboardDict.Remove(user.name);
                    leaderboardDict.Add(user.name, user.level);
                } // if (leaderboardDict.ContainsKey($"{user.name}"))
                else
                {
                    leaderboardDict.Add(user.name, user.level);
                } // if (leaderboardDict.ContainsKey($"{user.name}")) else

                string leaderboardjson = JsonConvert.SerializeObject(leaderboardDict, Formatting.Indented);

                File.WriteAllText($@"leaderboards/{ctx.Guild.Id}.json", leaderboardjson);
            }
        }

        public async Task LoadUser(string userid)
        {
            jsonplayer = File.ReadAllText($@"playersaves/{userid}.json");
            user = JsonConvert.DeserializeObject<Player>(jsonplayer);
        }

        public async Task startgame(CommandContext ctx)
        {
            Console.WriteLine($"{user.name} is in a level {user.level} game.");

            Enemy enemy = new Enemy(user.level); // creates an enemy depending on the user's level

            var enemyEmbed = new DiscordEmbedBuilder
            {
                Title = "An enemy appeared!",
                Description =
                $"Name: {enemy.name}\n" +
                $"Appearance: {enemy.app}\n" +
                $"Health: {enemy.health}\n" +
                $"Attack: {enemy.attack}\n" +
                $"Speed: {enemy.speed}\n",
                Color = DiscordColor.DarkRed
            };

            int phealth = user.health;
            int eohealth = enemy.health;
            int damage = 0;
            int enemydamage;
            int moveint = 0;
            string move;
            inFight = true;
            int xpGained;
            int moneyGained;
            Random rnd = new Random();

            var damageEmbed = new DiscordEmbedBuilder
            {
                Title = $"[damage info here]",
                Color = DiscordColor.Azure
            };

            System.Threading.Thread.Sleep(3000);

            await ctx.Channel.SendMessageAsync(embed: enemyEmbed).ConfigureAwait(false);

            var movesetEmbed = new DiscordEmbedBuilder
            {
                Title = "What will you do?\nYour moves:",
                Description =
                $"1. {user.moveset[0]}\n" +
                $"2. {user.moveset[1]}\n" +
                $"3. {user.moveset[2]}\n" +
                $"4. {user.moveset[3]}\n" +
                $"\nEnter the number of a move to use it.",
                Color = DiscordColor.Aquamarine
            };

            while (inFight)
            {
                move = "None";

                await ctx.Channel.SendMessageAsync(embed: movesetEmbed).ConfigureAwait(false);

                while (move == "None")
                {
                    var interactivity = ctx.Client.GetInteractivity();

                    var moveinput = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.User).ConfigureAwait(false);

                    moveint = System.Convert.ToInt32(moveinput.Result.Content);
                    if (moveint <= 4)
                    {
                        move = user.moveset[moveint - 1];
                        if (user.basemoves.Contains(move))
                        {
                            damage = System.Convert.ToInt32((user.attack * rnd.Next(15, 20)) / 100);
                        }
                        else if (user.tier2moves.Contains(move))
                        {
                            damage = System.Convert.ToInt32((user.attack * rnd.Next(30, 35)) / 100);
                        }
                        else if (user.tier3moves.Contains(move))
                        {
                            damage = System.Convert.ToInt32((user.attack * rnd.Next(35, 40)) / 100);
                        }
                        else if (user.tier4moves.Contains(move))
                        {
                            damage = System.Convert.ToInt32((user.attack * rnd.Next(40, 45)) / 100);
                        }
                        else if (user.tier5moves.Contains(move))
                        {
                            damage = System.Convert.ToInt32((user.attack * rnd.Next(45, 55)) / 100);
                        }
                    }
                }

                // Once a move is selected

                // Critical Hit
                
                int critprob = rnd.Next(1, 101);

                if (critprob <= user.critchance) // if it is a critical hit
                {
                    damage = System.Convert.ToInt32(damage * 1.5);
                } // if (critprob <= user.critchance)
                enemy.health -= damage;

                

                // Critical Hit

                if (enemy.health > 0) // if the enemy still has health
                {
                    damageEmbed.Title = $"Your {move} did {damage} damage!\n" +
                        $"The {enemy.name} now has {enemy.health}/{eohealth} health.";

                    System.Threading.Thread.Sleep(1000);

                    await ctx.Channel.SendMessageAsync(embed: damageEmbed).ConfigureAwait(false);

                    string enemymove;

                    if (user.level > 37)
                    {
                        enemymove = enemy.moveset[38, rnd.Next(0, 2)];
                    } // if (user.level > 37)
                    else
                    {
                        enemymove = enemy.moveset[rnd.Next(0, user.level - 1), rnd.Next(0, 2)];
                    } // if (user.level > 37) else

                    if (enemymove == "Small Heal")
                    {
                        int healthchange = eohealth / 5;
                        enemy.health += healthchange;
                        damageEmbed.Title = $"The {enemy.name} used {enemymove}, " +
                            $"healing by {healthchange}.\n" +
                            $"They now have {enemy.health} health.";
                    } // if (enemymove == "Small Heal")
                    else
                    {
                        enemydamage = System.Convert.ToInt32((enemy.attack * rnd.Next(21, 31)) / 100);

                        phealth -= enemydamage;
                        damageEmbed.Title = $"The {enemy.name} used {enemymove}, " +
                            $"dealing {enemydamage} damage!\n" +
                            $"You now have {phealth} health.";
                    } // if (enemymove == "Small Heal") else



                    System.Threading.Thread.Sleep(1000);
                    await ctx.Channel.SendMessageAsync(embed: damageEmbed).ConfigureAwait(false);
                } // if (enemy.health > 0)
                else
                {
                    damageEmbed.Title = $"Your {move} did {damage} damage, " +
                        $"defeating the {enemy.app} {enemy.name}!";
                    System.Threading.Thread.Sleep(1000);
                    await ctx.Channel.SendMessageAsync(embed: damageEmbed).ConfigureAwait(false);

                    double moneyMultiplier = rnd.Next(11, 26);

                    moneyGained = System.Convert.ToInt32((moneyMultiplier / 100) * eohealth);
                    user.balance += moneyGained;

                    double xpMultiplier = rnd.Next(11, 26);

                    xpGained = System.Convert.ToInt32((xpMultiplier / 100) * eohealth);

                    System.Threading.Thread.Sleep(500);

                    await addXp(xpGained, moneyGained, ctx);
                } // if (enemy.health > 0) else
                if (phealth <= 0)
                {
                    damageEmbed.Title = "You died!";
                    System.Threading.Thread.Sleep(1000);
                    await ctx.Channel.SendMessageAsync(embed: damageEmbed).ConfigureAwait(false);
                } // if (phealth <= 0)
                if (enemy.health <= 0 || phealth <= 0)
                {
                    inFight = false;
                } // if (enemy.health <= 0 || phealth <= 0)
            } // while (inFight)

            await Task.Run(() => save(ctx));
        } // public async Task startgame(CommandContext ctx)

        public async Task addXp(int amount, int moneyGained, CommandContext ctx)
        {
            user.currentXp += amount;
            var winEmbed = new DiscordEmbedBuilder
            {
                Title = $"Congratulations! You gained {amount} XP and ${moneyGained}! You now have ${user.balance}.",
                Color = DiscordColor.Yellow
            };

            await ctx.Channel.SendMessageAsync(embed: winEmbed).ConfigureAwait(false);

            if (user.currentXp >= user.targetXp)
            {
                user.currentXp -= user.targetXp;
                user.level++;
                user.targetXp += System.Convert.ToInt32(user.targetXp / 3); // increments target by 10% of the previous level's target
                var levelupEmbed = new DiscordEmbedBuilder
                {
                    Title = $"LEVEL UP!\n" +
                    $"You reached {user.level}!\n\n" +
                    $"You gained 1 skill point! Use !skills to use it!",
                    Color = DiscordColor.Purple
                };

                user.skillpoints++;

                await ctx.Channel.SendMessageAsync(embed: levelupEmbed).ConfigureAwait(false);
            } // if (user.currentXp >= user.targetXp)
            await Task.Run(() => save(ctx));
        } // public async Task addXp(int amount, CommandContext ctx)
    } // public class UtilityCommands : BaseCommandModule
} // namespace HexRPGDiscord.Commands