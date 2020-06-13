using System;
using System.IO;
using DSharpPlus;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using System.Collections.Generic;
using DSharpPlus.CommandsNext.Attributes;

namespace NoteMeSenpai
{
    public static class DiscordBot
    {
        private static DiscordClient _discord;
        private static CommandsNextExtension _commands;

        /// <summary>
        /// Inits all commands and events the bot has to and starts the bot loop 
        /// </summary>
        /// <param name="apiSecret">The discord bot API secret</param>
        /// <param name="prefixes">All prefixes the bot should react to</param>
        public static async Task Start(string apiSecret, List<string> prefixes)
        {
            Init(apiSecret, prefixes);
            await _discord.ConnectAsync();
            await Task.Delay(-1);
        }

        /// <summary>
        /// Inits all commands and events the bot has to 
        /// </summary>
        /// <param name="apiSecret">The discord bot API secret</param>
        /// <param name="prefixes">All prefixes the bot should react to</param>
        private static void Init(string apiSecret, List<string> prefixes)
        {
            _discord = new DiscordClient(new DiscordConfiguration
            {
                Token = apiSecret,
                TokenType = TokenType.Bot
            });

            
            var commandConfig = new CommandsNextConfiguration();
            commandConfig.StringPrefixes = prefixes;
            _commands = _discord.UseCommandsNext(commandConfig);

            // Hooks
            _commands.CommandExecuted += (commandExecutionEventArgs) =>
            {
                var ctx = commandExecutionEventArgs.Context;
                ctx.Message.DeleteAsync();
                return null;
            };

            // Add all commands
            _commands.RegisterCommands<Commands.Administration>();
            
        }

        /// <summary>
        /// Returns the discord client aka the bot
        /// </summary>
        /// <returns>DiscordClient</returns>
        public static DiscordClient GetDiscordClient() => _discord;

        
    }
}