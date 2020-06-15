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
        [Description("Adds a note.")]
        public async Task CreateNote(CommandContext ctx, string idOrName, string noteContent)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            if (Permissions.CheckCommandPermission(ctx))
            {
                if (DiscordBot.AddNote(idOrName, noteContent, ctx))
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", note for **" + idOrName + "** added.");
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

        [Command("note")]
        [Description("Adds a note.")]
        public async Task CreateNote(CommandContext ctx, string idOrName, params string[] noteContentList)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            var noteContent = string.Join(" ", noteContentList); 

            if (Permissions.CheckCommandPermission(ctx))
            {
                if (DiscordBot.AddNote(idOrName, noteContent, ctx))
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", note for **" + idOrName + "** added.");
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

        [Command("notes")]
        [Description("Displays all notes for a certain user.")]
        public async Task GetNotes(CommandContext ctx, string idOrName)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            if (Permissions.CheckCommandPermission(ctx))
            {
                if (!DiscordBot.GetNotes(idOrName, ctx))
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", that didn't work.");
                }
            }
            else
            {
                await DiscordBot.RespondAsync(ctx, mention + ", you do not have permission to do that.");
            }
        }

        [Command("allnotes")]
        [Description("Displays literally all notes, but as private message to prevent spam.")]
        public async Task GetAllNotes(CommandContext ctx)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            if (Permissions.CheckCommandPermission(ctx))
            {
                if (!DiscordBot.GetAllNotes(ctx))
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", that didn't work.");
                }
            }
            else
            {
                await DiscordBot.RespondAsync(ctx, mention + ", you do not have permission to do that.");
            }
        }

        [Command("delnote")]
        [Description("Deletes a single note.")]
        public async Task DeleteNote(CommandContext ctx, string noteId)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            if (Permissions.CheckCommandPermission(ctx))
            {
                if (DiscordBot.DeleteNote(noteId, ctx))
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", note **#" + noteId + "** deleted.");
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

        [Command("deleteallnotes")]
        [Description("Deletes all notes for a certain user.")]
        public async Task DeleteAllNotes(CommandContext ctx, string idOrName)
        {
            if (Permissions.CheckPrivate(ctx)) return;
            var mention = ctx.Member.Mention;
            if (Permissions.CheckCommandPermission(ctx))
            {
                if (DiscordBot.DeleteAllNotes(idOrName, ctx))
                {
                    await DiscordBot.RespondAsync(ctx, mention + ", all notes for **" + idOrName + "** deleted.");
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