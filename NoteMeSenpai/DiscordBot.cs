using System;
using System.IO;
using DSharpPlus;
using System.Linq;
using System.Linq.Expressions;
using NoteMeSenpai.Models;
using NoteMeSenpai.Database;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using System.Collections.Generic;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace NoteMeSenpai
{
    public static class DiscordBot
    {
        private static DiscordClient _discord;
        private static CommandsNextExtension _commands;
        private static DatabaseConnection _databaseConnection;

        private static Options _options;

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
        /// Adds a role that is allowed to execute commands
        /// </summary>
        /// <param name="role">Name of the role</param>
        /// <param name="command">Name of the command</param>
        /// <param name="guild">Guild object ToString()</param>
        public static bool AddRole(string role, string command, DiscordGuild guild)
        {
            if (!guild.Roles.Values.Select(x => x.Name).Contains(role))
            {
                return false;
            }
            Expression<Func<Permission, bool>> filter = (permission) => permission.Guild.Equals(guild.ToString()) && permission.Command.Equals(command) && permission.RoleName.Equals(role);
            var permissions = _databaseConnection.Get(filter);
            if(permissions == null)
            {
                permissions = new Permission();
                permissions.RoleName = role;
                permissions.Guild = guild.ToString();
                permissions.Command = command;
                _databaseConnection.Save(permissions);
            }
            return true;

        }

        /// <summary>
        /// Inits all commands and events the bot has to 
        /// </summary>
        /// <param name="apiSecret">The discord bot API secret</param>
        /// <param name="prefixes">All prefixes the bot should react to</param>
        private static void Init(string apiSecret, List<string> prefixes)
        {
            // Reading the Options
            _options = Options.LoadFromFile();

            if (string.IsNullOrWhiteSpace(_options.DatabaseConnectionString))
            {
                _options.DatabaseConnectionString = "mongodb://localhost:27017";
            }

            // Setup the database connection
            _databaseConnection = new DatabaseConnection(_options.DatabaseConnectionString);
            Util.Permissions.Init(_databaseConnection);

            // Setup the Discord client
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
            _commands.RegisterCommands<Commands.Notes>();
            _commands.RegisterCommands<Commands.Administration>();
            
        }

        /// <summary>
        /// Returns the discord client aka the bot
        /// </summary>
        /// <returns>DiscordClient</returns>
        public static DiscordClient GetDiscordClient() => _discord;

        
    }
}