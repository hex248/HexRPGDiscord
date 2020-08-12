﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using HexRPGDiscord.Commands;
using Newtonsoft.Json;

namespace HexRPGDiscord {
    class Bot {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public async Task RunAsync () {
            var json = string.Empty;

            using (var fs = File.OpenRead ("config.json"))
            using (var sr = new StreamReader (fs, new UTF8Encoding (false)))
            json = await sr.ReadToEndAsync ().ConfigureAwait (false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson> (json);

            var config = new DiscordConfiguration {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            };

            Client = new DiscordClient (config);

            Client.Ready += OnClientReady;

            Client.UseInteractivity (new InteractivityConfiguration {
                Timeout = TimeSpan.FromMinutes (2)
            });

            var commandsConfig = new CommandsNextConfiguration {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = false
            };

            Commands = Client.UseCommandsNext (commandsConfig);

            Commands.RegisterCommands<UtilityCommands> ();

            await Client.ConnectAsync ();

            await Task.Delay (-1);
        }

        private Task OnClientReady (ReadyEventArgs e) {
            Console.WriteLine ("The bot is online.");
            Console.WriteLine ("Waiting for commands...");
            return null;
        }
    }
}