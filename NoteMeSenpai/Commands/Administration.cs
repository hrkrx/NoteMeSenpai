using System;
using NoteMeSenpai.Util;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace NoteMeSenpai.Commands
{
    public class Administration : BaseCommandModule
    {
        [Command("status")]
        [Description("Changes the status of the bot.")]
        public async Task Status(CommandContext ctx, string status)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            if (Permissions.CheckCommandPermission(ctx))
            {
                await DiscordBot.GetDiscordClient().UpdateStatusAsync(new DSharpPlus.Entities.DiscordActivity(status.Trim()));
            }
            else
            {
                var mention = ctx.Member.Mention;
                await DiscordBot.RespondAsync(ctx, mention + ", you do not have permission to do that.");
            }
        }

        [Command("status")]
        [Description("Changes the status of the bot.")]
        public async Task Status(CommandContext ctx, params string[] statusList)
        {
            var status = string.Join(" ", statusList);
            if (Permissions.CheckPrivate(ctx)) return;
            if (Permissions.CheckCommandPermission(ctx))
            {
                await DiscordBot.GetDiscordClient().UpdateStatusAsync(new DSharpPlus.Entities.DiscordActivity(status.Trim()));
            }
            else
            {
                var mention = ctx.Member.Mention;
                await DiscordBot.RespondAsync(ctx, mention + ", you do not have permission to do that.");
            }
        }

        [Command("addrole")]
        [Description("Adds a role to the group of privileged roles for a specific command only.")]
        public async Task AddRole(CommandContext ctx, string role, string command)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            if (Permissions.CheckCommandPermission(ctx))
            {
                if (DiscordBot.AddRole(role, command, ctx.Guild))
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", role **" + role + "** added for *" + command + "*");
                }
                else
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", that didn't work.");
                }
            }
            else
            {
                await DiscordBot.RespondAsync(ctx, mention + ", you do not have permission to do that.");
            }
        }

        [Command("addrole")]
        [Description("Adds a role to the group of privileged roles for all commands.")]
        public async Task AddRole(CommandContext ctx, string role)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            if (Permissions.CheckCommandPermission(ctx))
            {
                if (DiscordBot.AddRole(role, "*", ctx.Guild))
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", role **" + role + "** added for *all commands*");
                }
                else
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", that didn't work.");
                }
            }
            else
            {
                await DiscordBot.RespondAsync(ctx, mention + ", you do not have permission to do that.");
            }
        }

        [Command("removerole")]
        [Description("Removes a role from the group of privileged roles for a specific commands.")]
        public async Task RemoveRole(CommandContext ctx, string role, string command)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            if (Permissions.CheckCommandPermission(ctx))
            {
                if (DiscordBot.RemoveRole(role, command, ctx.Guild))
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", role **" + role + "** removed for *" + command + "*");
                }
                else
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", that didn't work.");
                }
            }
            else
            {
                await DiscordBot.RespondAsync(ctx, mention + ", you do not have permission to do that.");
            }
        }

        [Command("removerole")]
        [Description("Removes a role from the group of privileged roles for all commands.")]
        public async Task RemoveRole(CommandContext ctx, string role)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            if (Permissions.CheckCommandPermission(ctx))
            {
                if (DiscordBot.RemoveRole(role, "*", ctx.Guild))
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", role **" + role + "** removed for *all commands*");
                }
                else
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", that didn't work.");
                }
            }
            else
            {
                await DiscordBot.RespondAsync(ctx, mention + ", you do not have permission to do that.");
            }
        }

        [Command("addchannel")]
        [Description("Adds a channel to the valid response channels")]
        public async Task AddChannel(CommandContext ctx, string channelIDorName)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            var channel = ctx.Guild.Channels.Values.FirstOrDefault(x => x.Name.Equals(channelIDorName) || x.Id.ToString().Equals(channelIDorName));
            
            if (Permissions.CheckCommandPermission(ctx))
            {
                if (DiscordBot.AddChannel(channel))
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", channel **" + channelIDorName + "** added as valid response channel");
                }
                else
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", that didn't work.");
                }
            }
            else
            {
                await DiscordBot.RespondAsync(ctx, mention + ", you do not have permission to do that.");
            }
        }

        [Command("setdefaultchannel")]
        [Description("Adds or sets a channel to the valid response channels as preferred channel.")]
        public async Task SetDefaultChannel(CommandContext ctx, string channelIDorName)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            var channel = ctx.Guild.Channels.Values.FirstOrDefault(x => x.Name.Equals(channelIDorName) || x.Id.ToString().Equals(channelIDorName));
            
            if (Permissions.CheckCommandPermission(ctx))
            {
                if (DiscordBot.SetDefaultChannel(channel))
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", channel **" + channelIDorName + "** set as default response channel");
                }
                else
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", that didn't work.");
                }
            }
            else
            {
                await DiscordBot.RespondAsync(ctx, mention + ", you do not have permission to do that.");
            }
        }

        [Command("removechannel")]
        [Description("Removes a channel from0 the valid response channels.")]
        public async Task RemoveChannel(CommandContext ctx, string channelIDorName)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            var channel = ctx.Guild.Channels.Values.FirstOrDefault(x => x.Name.Equals(channelIDorName) || x.Id.ToString().Equals(channelIDorName));
            
            if (Permissions.CheckCommandPermission(ctx))
            {
                if (DiscordBot.RemoveChannel(channel))
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", channel **" + channelIDorName + "** removed from valid response channels");
                }
                else
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", that didn't work.");
                }
            }
            else
            {
                await DiscordBot.RespondAsync(ctx, mention + ", you do not have permission to do that.");
            }
        }

        [Command("purge")]
        [Description("Removes all messages send by the bot")]
        public async Task PurgeAllMessages(CommandContext ctx)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            if (Permissions.CheckCommandPermission(ctx))
            {
                if (!DiscordBot.PurgeAllMessages(ctx))
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", that didn't work.");
                }
            }
            else
            {
                await DiscordBot.RespondAsync(ctx, mention + ", you do not have permission to do that.");
            }
        }

        [Command("setprefix")]
        [Description("Changes the prefix (one or more allowed)")]
        public async Task SetPrefix(CommandContext ctx, params string[] prefixes)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            if (Permissions.CheckCommandPermission(ctx))
            {
                if (!DiscordBot.SetPrefixes(ctx, prefixes))
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", that didn't work.");
                }
                else
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", prefixes changed to: " + string.Join(", ", DiscordBot.GetPrefixes().Select(x => "\"" + x + "\"")));
                }
            }
            else
            {
                await DiscordBot.RespondAsync(ctx, mention + ", you do not have permission to do that.");
            }
        }

        [Command("setdeletiondelay")]
        [Description("Changes delay in seconds before a message from the bot is removed")]
        public async Task SetDeletionDelay(CommandContext ctx, int delay)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            if (Permissions.CheckCommandPermission(ctx))
            {
                if (!DiscordBot.SetDeletionDelay(ctx, delay))
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", that didn't work.");
                }
                else
                {
                    var newDelay = DiscordBot.GetOptions().DeletionDelayInSeconds;
                    await DiscordBot.RespondAsync(ctx, mention + ", deletion delay set to " + newDelay);
                }
            }
            else
            {
                await DiscordBot.RespondAsync(ctx, mention + ", you do not have permission to do that.");
            }
        }

        [Command("update")]
        [Description("Updates the bot to the latest version available on github. only works if setup correctly (e.g. with the provided docker image)")]
        public async Task Update(CommandContext ctx)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            if (Permissions.CheckCommandPermission(ctx))
            {
                var newDelay = DiscordBot.GetOptions().DeletionDelayInSeconds;
                await DiscordBot.RespondAsync(ctx, mention + ", Bot is shutting down in " + newDelay + " seconds to update.");
                await ctx.Message.DeleteAsync();
                Task.Delay(newDelay * 1000 + 500).GetAwaiter().GetResult();
                DiscordBot.Shutdown();
            }
            else
            {
                await DiscordBot.RespondAsync(ctx, mention + ", you do not have permission to do that.");
            }
        }

        [Command("version")]
        [Description("gets the assembly version")]
        public async Task GetVersion(CommandContext ctx)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            if (Permissions.CheckCommandPermission(ctx))
            {
                await DiscordBot.RespondAsync(ctx, mention + ", Version: " + typeof(Program).Assembly.GetName().Version);
            }
            else
            {
                await DiscordBot.RespondAsync(ctx, mention + ", you do not have permission to do that.");
            }
        }

        [Command("showconfig")]
        [Description("Displays the entire configuration for this server")]
        public async Task ShowConfig(CommandContext ctx)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            if (Permissions.CheckCommandPermission(ctx))
            {
                var options = DiscordBot.GetOptions();
                var message = $"Configuration for *{ctx.Guild.Name}*:\n";
                var channels = DiscordBot.GetChannels(ctx.Guild);
                var permissions = Util.Permissions.GetPermissions(ctx.Guild);

                message += "```\n";
                message += $"DatabaseConnection = {options.DatabaseConnectionString}\n";
                message += $"Prefixes = {string.Join(", ", options.Prefixes.Select(x => "\"" + x + "\""))}\n";
                message += $"DeletionDelay = {options.DeletionDelayInSeconds}\n";
                if (channels.Count() > 0)
                {
                    message += $"Channels the bot is allowed to post in:\n\n";
                    message += $"{string.Join("\n", channels.Select(x => x.IsDefault ? x.ChannelName + " (default)" : x.ChannelName))}\n";
                }
                else
                {
                    message += $"No channelrestrictions set.\n";
                }
                message += $"\n";
                
                if (permissions.Count() > 0)
                {
                    var grouping = permissions.GroupBy(x => x.RoleName);
                    message += $"Permissions:\n\n";
                    foreach (var permissiongroup in grouping)
                    {
                        message += $"{permissiongroup.First().RoleName}:\n";
                        message += $"\t -> {string.Join("\n", permissiongroup.Select(x => x.Command))}\n"; 
                        message += $"\n";
                    }
                }
                else
                {
                    message += $"No permissions set.\n";
                }
                message += "```\n";
                await DiscordBot.RespondAsync(ctx, mention + "\n" + message);
            }
            else
            {
                await DiscordBot.RespondAsync(ctx, mention + ", you do not have permission to do that.");
            }
        }
    }
}