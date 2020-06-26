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
using System.Reflection;

namespace NoteMeSenpai
{
    public static class DiscordBot
    {
        private static DiscordClient _discord;
        private static CommandsNextExtension _commands;
        private static DatabaseConnection _databaseConnection;
        private static CommandsNextConfiguration _config;
        private static Options _options;


        /// <summary>
        /// Inits all commands and events the bot has to and starts the bot loop 
        /// </summary>
        /// <param name="apiSecret">The discord bot API secret</param>
        /// <param name="prefixes">All prefixes the bot should react to</param>
        public static async Task Start(string apiSecret, bool waitInfinitely = true)
        {
            Init(apiSecret);
            await _discord.ConnectAsync();
            if (waitInfinitely)
            {
                await Task.Delay(-1);
            }
        }

        internal static Options GetOptions() => _options;

        /// <summary>
        /// Inits all commands and events the bot has to 
        /// </summary>
        /// <param name="apiSecret">The discord bot API secret</param>
        /// <param name="prefixes">All prefixes the bot should react to</param>
        private static void Init(string apiSecret)
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

            
            _config = new CommandsNextConfiguration();
            _config.StringPrefixes = _options.Prefixes;
            _commands = _discord.UseCommandsNext(_config);

            // Hooks
            _commands.CommandExecuted += (commandExecutionEventArgs) =>
            {
                var ctx = commandExecutionEventArgs.Context;
                ctx.Message.DeleteAsync();
                return null;
            };

            _discord.MessageCreated += (messageCreatedEventArgs) =>
            {
                var isThisBot = messageCreatedEventArgs.Author.Id == _discord.CurrentUser.Id;
                if (isThisBot)
                {
                    new Task(() => {
                        Task.Delay(_options.DeletionDelayInSeconds * 1000).GetAwaiter().GetResult();
                        messageCreatedEventArgs.Message.DeleteAsync();
                    }).Start();
                }
                
                return null;
            };

            // Add all commands
            _commands.RegisterCommands<Commands.Notes>();
            _commands.RegisterCommands<Commands.Administration>();
            
        }

        /// <summary>
        /// Get all notes on a single User
        /// </summary>
        /// <param name="idOrName">The id or the full name</param>
        /// <param name="ctx">Command context</param>
        /// <returns></returns>
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

        /// <summary>
        /// Adds a new note on a User
        /// </summary>
        /// <param name="idOrName">Id or name</param>
        /// <param name="noteContent">The note</param>
        /// <param name="ctx">Command context</param>
        /// <returns></returns>
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

        /// <summary>
        /// Deletes a note by it's ID
        /// </summary>
        /// <param name="noteId">note Id</param>
        /// <param name="ctx">Command context</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets all notes (paginated)
        /// </summary>
        /// <param name="ctx">Command context</param>
        /// <param name="from">start id</param>
        /// <param name="to">end id</param>
        /// <returns></returns>
        public static bool GetAllNotes(CommandContext ctx, int from, int to)
        {
            Expression<Func<Note, bool>> filter = note => note.Guild.Equals(ctx.Guild.ToString());
            var notes = _databaseConnection.GetAll(filter).Where(x => x.NoteID >= from && x.NoteID < to).OrderBy(x => x.NoteID);
            var message = "";

            if (notes.Count() == 0)
            {
                RespondAsync(ctx, ctx.Member.Mention + ", no more notes.").GetAwaiter();
            }
            else
            {
                foreach (var note in notes)
                {
                    if (message.Length + note.ToDiscordString().Length >= 2000)
                    {
                        RespondAsync(ctx, message).GetAwaiter();
                        message = "";
                    }
                    message += note.ToDiscordString();

                }
                message += ctx.Member.Mention + ", use the *next* command to show the next 25 notes";
                RespondAsync(ctx, message).GetAwaiter();
            }
            return true;
        }

        /// <summary>
        /// Deletes all notes on a specific user
        /// </summary>
        /// <param name="idOrName">Id or Name</param>
        /// <param name="ctx">Command context</param>
        /// <returns></returns>
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
        /// Sets the deletion delay in seconds before a message is deleted
        /// </summary>
        /// <param name="ctx">Command context</param>
        /// <param name="delay">Delay in seconds</param>
        /// <returns></returns>
        internal static bool SetDeletionDelay(CommandContext ctx, int delay)
        {
            _options.DeletionDelayInSeconds = delay;
            Options.SaveToFile(_options, "settings.json");
            return true;
        }

        /// <summary>
        /// Returns the current prefixes
        /// </summary>
        /// <returns>string array</returns>
        public static string[] GetPrefixes()
        {
            var property = typeof(CommandsNextConfiguration).GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).FirstOrDefault(x => x.Name.Equals("StringPrefixes"));
            return ((IEnumerable<string>)property.GetValue(_config)).ToArray();
        }

        /// <summary>
        /// Sets the Prefixes and restarts the Bot
        /// </summary>
        /// <param name="ctx">Command context</param>
        /// <param name="prefixes">all useable prefixes</param>
        /// <returns></returns>
        internal static bool SetPrefixes(CommandContext ctx, string[] prefixes)
        {
            ctx.Message.DeleteAsync().GetAwaiter().GetResult();
            _discord.DisconnectAsync().GetAwaiter().GetResult();
            _discord.Dispose();
            Task.Delay(1000).GetAwaiter();

            _options.Prefixes = prefixes.ToList();
            Options.SaveToFile(_options, "settings.json");

            string apiKey = "<BOT-API-SECRET>";
            if (File.Exists("api.key")) apiKey = File.ReadAllText("api.key");
            DiscordBot.Start(apiKey, false).ConfigureAwait(false);

            Task.Delay(1000).GetAwaiter();
            return true;
        }

        /// <summary>
        /// Removes all messages the bot has written from the server
        /// </summary>
        /// <param name="ctx">Command context</param>
        /// <returns></returns>
        internal static bool PurgeAllMessages(CommandContext ctx)
        {
            try
            {
                foreach (var channel in ctx.Guild.Channels.Values.Where(x => x.Type == ChannelType.Text))
                {
                    foreach (var message in channel.GetMessagesBeforeAsync(channel.LastMessageId).GetAwaiter().GetResult())
                    {
                        if (message.Author.Id == _discord.CurrentUser.Id)
                        {
                            message.DeleteAsync();   
                        }
                    }
                }
                return true;
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

        /// <summary>
        /// Adds a Channel to the list of allowed response channels
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Sets the default channel for responding
        /// </summary>
        /// <param name="channel">Channel</param>
        /// <returns></returns>
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

        /// <summary>
        /// Removes a channel from the allowed channel list
        /// </summary>
        /// <param name="channel">Channel</param>
        /// <returns></returns>
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

        /// <summary>
        /// Shutdown bot and exit program
        /// </summary>
        public static void Shutdown()
        {
            _discord.DisconnectAsync().GetAwaiter().GetResult();
            _discord.Dispose();
            Environment.Exit(0);
        }
    }
}