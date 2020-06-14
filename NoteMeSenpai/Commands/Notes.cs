using System;
using System.Linq;
using NoteMeSenpai.Util;
using System.Threading.Tasks;
using System.Collections.Generic;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace NoteMeSenpai.Commands
{
    public class Notes : BaseCommandModule
    {        
        [Command("note")]
        public async Task CreateNote(CommandContext ctx, string IDorName, string noteContent)
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

        [Command("notes")]
        public async Task GetNotes(CommandContext ctx, string IDorName)
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

        [Command("allnotes")]
        public async Task GetAllNotes(CommandContext ctx)
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

        [Command("delnote")]
        public async Task DeleteNote(CommandContext ctx, string noteId)
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

        [Command("deleteallnotes")]
        public async Task DeleteAllNotes(CommandContext ctx)
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