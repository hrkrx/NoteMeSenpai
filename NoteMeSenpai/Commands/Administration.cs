using System;
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
        public static Dictionary<string,List<string>> RequiredRoleName;

        [Command("status")]
        public async Task Status(CommandContext ctx, string status)
        {
            // if () != null)
            // {
            //     await DiscordBot.GetDiscordClient().UpdateStatusAsync(new DSharpPlus.Entities.DiscordActivity(status.Trim()));
            // }
            await ctx.RespondAsync("Not yet implemented");
        }

        [Command("addrole")]
        public async Task AddRole(CommandContext ctx, string role, string command)
        {
            await ctx.RespondAsync("Not yet implemented");
        }

        [Command("removerole")]
        public async Task RemoveRole(CommandContext ctx, string role, string command)
        {
            await ctx.RespondAsync("Not yet implemented");
        }

        [Command("addchannel")]
        public async Task AddChannel(CommandContext ctx, string channelIDorName)
        {
            await ctx.RespondAsync("Not yet implemented");
        }

        [Command("setdefaultchannel")]
        public async Task SetDefaultChannel(CommandContext ctx, string channelIDorName)
        {
            await ctx.RespondAsync("Not yet implemented");
        }

        [Command("removechannel")]
        public async Task RemoveChannel(CommandContext ctx, string channelIDorName)
        {
            await ctx.RespondAsync("Not yet implemented");
        }
    }
}