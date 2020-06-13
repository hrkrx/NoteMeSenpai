using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace NoteMeSenpai.Commands
{
    public class Administration : BaseCommandModule
    {
        private const string RequiredRoleName = "Discord Mod";

        [Command("status")]
        public async Task ResetAndUpdate(CommandContext ctx, string status)
        {
            if (ctx.Member.Roles.FirstOrDefault(x => x.Name.Equals(RequiredRoleName)) != null)
            {
                await DiscordBot.GetDiscordClient().UpdateStatusAsync(new DSharpPlus.Entities.DiscordActivity(status.Trim()));
            }
        }
    }
}