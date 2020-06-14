using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace NoteMeSenpai.Commands
{
    public class Notes : BaseCommandModule
    {
        public static Dictionary<string,List<string>> RequiredRoleName;
        
        [Command("note")]
        public async Task CreateNote(CommandContext ctx, string IDorName, string noteContent)
        {
            await ctx.RespondAsync("Not yet implemented");
        }

        [Command("notes")]
        public async Task GetNotes(CommandContext ctx, string IDorName)
        {
            await ctx.RespondAsync("Not yet implemented");
        }

        [Command("allnotes")]
        public async Task GetAllNotes(CommandContext ctx)
        {
            await ctx.RespondAsync("Not yet implemented");
        }

        [Command("delnote")]
        public async Task DeleteNote(CommandContext ctx, string noteId)
        {
            await ctx.RespondAsync("Not yet implemented");
        }

        [Command("deleteallnotes")]
        public async Task DeleteAllNotes(CommandContext ctx)
        {
            await ctx.RespondAsync("Not yet implemented");
        }

    }
}