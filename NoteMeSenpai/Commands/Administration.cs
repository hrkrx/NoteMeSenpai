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
    }
}