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


        public static bool GetNotes(string idOrName, CommandContext ctx)
        {
            var userById = ctx.Guild.Members.FirstOrDefault(x => x.Value.Id.ToString().Equals(idOrName)).Value;
            var multiplePossibleUser = ctx.Guild.Members.Values.Where(x => x.DisplayName.Equals(idOrName));
            if (userById != null)
            {
                Expression<Func<Note, bool>> filter = note => note.Guild.Equals(ctx.Guild.ToString()) && note.TargetID.Equals(userById.Id.ToString());
                var notes = _databaseConnection.GetAll(filter);
                foreach (var note in notes)
                {
                    RespondAsync(ctx, note.ToDiscordString()).GetAwaiter();
                }
                if (notes.Count() < 1)
                {
                    RespondAsync(ctx, "No notes on user **" + idOrName + "** found").GetAwaiter();
                }
                return true;
            }

            if (multiplePossibleUser == null)
            {
                RespondAsync(ctx, "No user with the name/id **" + idOrName + "** found.").GetAwaiter();
                return false;
            }
            else if (multiplePossibleUser.Count() > 1)
            {
                RespondAsync(ctx, "Multiple user with the same name (**" + idOrName + "**) found.").GetAwaiter();
                return false;
            }
            else if (multiplePossibleUser.Count() == 0)
            {
                RespondAsync(ctx, "No user with the name/id **" + idOrName + "** found.").GetAwaiter();
                return false;
            }
            else
            {
                var user = multiplePossibleUser.FirstOrDefault();
                Expression<Func<Note, bool>> filter = note => note.Guild.Equals(ctx.Guild.ToString()) && note.TargetID.Equals(user.Id.ToString());
                var notes = _databaseConnection.GetAll(filter);
                foreach (var note in notes)
                {
                    RespondAsync(ctx, note.ToDiscordString()).GetAwaiter();
                }
                if (notes.Count() < 1)
                {
                    RespondAsync(ctx, "No notes on user **" + idOrName + "** found").GetAwaiter();
                }
                return true;
            }
        }

        public static bool AddNote(string idOrName, string noteContent, CommandContext ctx)
        {
            var userById = ctx.Guild.Members.FirstOrDefault(x => x.Value.Id.ToString().Equals(idOrName)).Value;
            var multiplePossibleUser = ctx.Guild.Members.Values.Where(x => x.DisplayName.Equals(idOrName));
            Expression<Func<Note, bool>> filter = note => note.Guild.Equals(ctx.Guild.ToString());
            var noteCount = _databaseConnection.GetAll(filter).Count() + 1;

            if (userById != null)
            {
                var note = new Note(ctx.Member.DisplayName, userById.Id.ToString(), userById.DisplayName, noteContent, ctx.Guild.ToString());
                note.NoteID = noteCount;
                _databaseConnection.Save(note);
                return true;
            }

            if (multiplePossibleUser == null)
            {
                RespondAsync(ctx, "No user with the name/id **" + idOrName + "** found.").GetAwaiter();
                return false;
            }
            else if (multiplePossibleUser.Count() > 1)
            {
                RespondAsync(ctx, "Multiple user with the same name (**" + idOrName + "**) found.").GetAwaiter();
                return false;
            }
            else if (multiplePossibleUser.Count() == 0)
            {
                RespondAsync(ctx, "No user with the name **" + idOrName + "** found.").GetAwaiter();
                return false;
            }
            else
            {
                var user = multiplePossibleUser.FirstOrDefault();
                var note = new Note(ctx.Member.DisplayName, user.Id.ToString(), user.DisplayName, noteContent, ctx.Guild.ToString());
                note.NoteID = noteCount;
                _databaseConnection.Save(note);
                return true;
            }
        }

        public static bool DeleteNote(string noteId, CommandContext ctx)
        {
            try
            {
                var id = long.Parse(noteId);
                Expression<Func<Note, bool>> filter = note => note.NoteID == id;
                var note = _databaseConnection.Get(filter);
                _databaseConnection.Delete(note);
                return true;
            }
            catch (System.Exception)
            {
                return false;            
            }
        }

        public static bool GetAllNotes(CommandContext ctx)
        {
            Expression<Func<Note, bool>> filter = note => note.Guild.Equals(ctx.Guild.ToString());
            var notes = _databaseConnection.GetAll(filter);
            foreach (var note in notes)
            {
                ctx.Member.SendMessageAsync(note.ToDiscordString()).GetAwaiter();
            }
            return true;
        }

        public static bool DeleteAllNotes(string idOrName, CommandContext ctx)
        {
            try
            {
                var userById = ctx.Guild.Members.FirstOrDefault(x => x.Value.Id.ToString().Equals(idOrName)).Value;
                var multiplePossibleUser = ctx.Guild.Members.Values.Where(x => x.DisplayName.Equals(idOrName));
                if (userById != null)
                {
                    Expression<Func<Note, bool>> filter = note => note.Guild.Equals(ctx.Guild.ToString()) && note.TargetID.Equals(userById.Id.ToString());
                    var notes = _databaseConnection.GetAll(filter);
                    foreach (var note in notes)
                    {
                        _databaseConnection.Delete(note);
                    }
                    return true;
                }

                if (multiplePossibleUser == null)
                {
                    RespondAsync(ctx, "No user with the name/id **" + idOrName + "** found.").GetAwaiter();
                    return false;
                }
                else if (multiplePossibleUser.Count() > 1)
                {
                    RespondAsync(ctx, "Multiple user with the same name (**" + idOrName + "**) found.").GetAwaiter();
                    return false;
                }
                else if (multiplePossibleUser.Count() == 0)
                {
                    RespondAsync(ctx, "No user with the name/id **" + idOrName + "** found.").GetAwaiter();
                    return false;
                }
                else
                {
                    var user = multiplePossibleUser.FirstOrDefault();
                    Expression<Func<Note, bool>> filter = note => note.Guild.Equals(ctx.Guild.ToString()) && note.TargetID.Equals(user.Id.ToString());
                    var notes = _databaseConnection.GetAll(filter);
                    foreach (var note in notes)
                    {
                        _databaseConnection.Delete(note);
                    }
                    return true;
                }
            }
            catch (System.Exception)
            {
                return false;            
            }
        }

        /// <summary>
        /// Adds a role that is allowed to execute commands. "*" for wildcard
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
        /// Removes a role from a command. "*" for wildcard
        /// </summary>
        /// <param name="role"></param>
        /// <param name="command"></param>
        public static bool RemoveRole(string role, string command, DiscordGuild guild)
        {
            if (!guild.Roles.Values.Select(x => x.Name).Contains(role))
            {
                return false;
            }

            Expression<Func<Permission, bool>> filter = (permission) => permission.Guild.Equals(guild.ToString()) && permission.Command.Equals(command) && permission.RoleName.Equals(role);
            var permissions = _databaseConnection.GetAll(filter);

            foreach (var permission in permissions)
            {
                _databaseConnection.Delete(permission);
            }

            return true;
        }

        public static bool AddChannel(DiscordChannel channel)
        {
            if (channel == null)
            {
                return false;
            }

            Expression<Func<Channel, bool>> filter = c => c.Guild.Equals(channel.Guild.ToString()) && c.ChannelId.Equals(channel.Id.ToString());
            var validChannel = _databaseConnection.Get(filter);
            if(validChannel == null)
            {
                validChannel = new Channel();
                validChannel.ChannelId = channel.Id.ToString();
                validChannel.Guild = channel.Guild.ToString();
                validChannel.ChannelName = channel.Name;
                validChannel.IsDefault = false;
                _databaseConnection.Save(validChannel);
            }
            return true;
        }

        /// <summary>
        /// Returns the discord client aka the bot
        /// </summary>
        /// <returns>DiscordClient</returns>
        public static DiscordClient GetDiscordClient() => _discord;

        public static async Task RespondAsync(CommandContext context, string message)
        {
            var mention = context.Member.Mention;
            var channels = context.Guild.Channels;
            
            Expression<Func<Channel, bool>> filter = channel => channel.Guild.Equals(context.Guild.ToString());
            var allowedChannels = _databaseConnection.GetAll(filter);
            var responseInSameChannelAllowed = allowedChannels.FirstOrDefault(x => x.ChannelId.Equals(context.Channel.Id.ToString())) != null;
            if (responseInSameChannelAllowed || allowedChannels.Count() == 0)
            {
                await context.RespondAsync(message);
            }
            else
            {
                var defaultChannel = allowedChannels.FirstOrDefault(x => x.IsDefault);
                if (defaultChannel == null)
                {
                    defaultChannel = allowedChannels.FirstOrDefault();
                }

                var responseChannel = channels.Values.FirstOrDefault(x => defaultChannel.ChannelId.Equals(x.Id.ToString()));

                await responseChannel.SendMessageAsync(mention + " you have requested a command from **" + context.Channel.Name + "**, to make it a valid response channel use *-addchannel " + context.Channel.Name + "*\n");
                await responseChannel.SendMessageAsync(message);
            }
        }

        public static bool SetDefaultChannel(DiscordChannel channel)
        {
            if (channel == null)
            {
                return false;
            }

            Expression<Func<Channel, bool>> filter = c => c.Guild.Equals(channel.Guild.ToString()) && c.ChannelId.Equals(channel.Id.ToString());
            Expression<Func<Channel, bool>> defaultFilter = c => c.Guild.Equals(channel.Guild.ToString()) && c.IsDefault;
            var validChannel = _databaseConnection.Get(filter);
            var defaultChannel = _databaseConnection.GetAll(defaultFilter);

            if(validChannel == null)
            {
                validChannel = new Channel();
                validChannel.ChannelId = channel.Id.ToString();
                validChannel.Guild = channel.Guild.ToString();
                validChannel.ChannelName = channel.Name;
                validChannel.IsDefault = true;
                _databaseConnection.Save(validChannel);
            }
            else
            {
                validChannel.IsDefault = true;
                _databaseConnection.Update(validChannel);
            }

            foreach (var defChannel in defaultChannel)
            {
                defChannel.IsDefault = false;
                _databaseConnection.Update(defChannel);
            }

            return true;
        }

        public static bool RemoveChannel(DiscordChannel channel)
        {
            if (channel == null)
            {
                return false;
            }

            Expression<Func<Channel, bool>> filter = c => c.Guild.Equals(channel.Guild.ToString()) && c.ChannelId.Equals(channel.Id.ToString());
            var toBePurged = _databaseConnection.GetAll(filter);

            foreach (var permission in toBePurged)
            {
                _databaseConnection.Delete(permission);
            }

            return true;
        }
    }
}