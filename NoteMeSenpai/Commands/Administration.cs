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
        public async Task Status(CommandContext ctx, string status)
        {
            if (Permissions.CheckCommandPermission(ctx))
            {
                await DiscordBot.GetDiscordClient().UpdateStatusAsync(new DSharpPlus.Entities.DiscordActivity(status.Trim()));
            }
            else
            {
                var mention = ctx.Member.Mention;
                await ctx.RespondAsync(mention + ", you do not have permission to do that.");
            }
        }

        [Command("addrole")]
        public async Task AddRole(CommandContext ctx, string role, string command)
        {
            if (Permissions.CheckCommandPermission(ctx))
            {
                await ctx.RespondAsync("Not yet implemented");
            }
            else
            {
                var mention = ctx.Member.Mention;
                await ctx.RespondAsync(mention + ", you do not have permission to do that.");
            }
        }

        [Command("addrole")]
        public async Task AddRole(CommandContext ctx, string role)
        {
            if (Permissions.CheckCommandPermission(ctx))
            {
                await ctx.RespondAsync("Not yet implemented");
            }
            else
            {
                var mention = ctx.Member.Mention;
                await ctx.RespondAsync(mention + ", you do not have permission to do that.");
            }
        }

        [Command("removerole")]
        public async Task RemoveRole(CommandContext ctx, string role, string command)
        {
            if (Permissions.CheckCommandPermission(ctx))
            {
                await ctx.RespondAsync("Not yet implemented");
            }
            else
            {
                var mention = ctx.Member.Mention;
                await ctx.RespondAsync(mention + ", you do not have permission to do that.");
            }
        }

        [Command("removerole")]
        public async Task RemoveRole(CommandContext ctx, string role)
        {
            if (Permissions.CheckCommandPermission(ctx))
            {
                await ctx.RespondAsync("Not yet implemented");
            }
            else
            {
                var mention = ctx.Member.Mention;
                await ctx.RespondAsync(mention + ", you do not have permission to do that.");
            }
        }

        [Command("addchannel")]
        public async Task AddChannel(CommandContext ctx, string channelIDorName)
        {
            if (Permissions.CheckCommandPermission(ctx))
            {
                await ctx.RespondAsync("Not yet implemented");
            }
            else
            {
                var mention = ctx.Member.Mention;
                await ctx.RespondAsync(mention + ", you do not have permission to do that.");
            }
        }

        [Command("setdefaultchannel")]
        public async Task SetDefaultChannel(CommandContext ctx, string channelIDorName)
        {
            if (Permissions.CheckCommandPermission(ctx))
            {
                await ctx.RespondAsync("Not yet implemented");
            }
            else
            {
                var mention = ctx.Member.Mention;
                await ctx.RespondAsync(mention + ", you do not have permission to do that.");
            }
        }

        [Command("removechannel")]
        public async Task RemoveChannel(CommandContext ctx, string channelIDorName)
        {
            if (Permissions.CheckCommandPermission(ctx))
            {
                await ctx.RespondAsync("Not yet implemented");
            }
            else
            {
                var mention = ctx.Member.Mention;
                await ctx.RespondAsync(mention + ", you do not have permission to do that.");
            }
        }
    }
}