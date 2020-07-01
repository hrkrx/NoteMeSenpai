using System;
using System.Linq;
using NoteMeSenpai.Models;
using System.Linq.Expressions;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using NoteMeSenpai.Database;
using System.Collections.Generic;

namespace NoteMeSenpai.Util
{
    public static class Permissions
    {

        private static DatabaseConnection _databaseConnection;

        public static void Init(DatabaseConnection dbConn)
        {
            _databaseConnection = dbConn;
        }

        public static bool CheckPrivate(CommandContext context)
        {
            if (context.Guild == null)
            {
                context.RespondAsync("You seem to like this Bot, but it can't like you back as you'd love to. So the developer put this message here to save you from suffering.");
                return true;
            }
            return false;
        }
        public static bool CheckCommandPermission(CommandContext context)
        {
            if (_databaseConnection == null)
            {
                Console.Error.WriteLine("Permission denied! Permission database connection not initialized!");
                return false;
            }

            if (context.Guild.Owner.Id.Equals(context.Member.Id))
            {
                return true;
            }

            var permissions = GetPermissions(context.Guild);
            
            var notYetRegulated = permissions.Where(x => x.Command.Equals(context.Command.Name) || x.Command.Equals("*")).Count() == 0;
            var specificallyAllowed = permissions.FirstOrDefault(x => context.Member.Roles.Select(r => r.Name).Contains(x.RoleName) && context.Command.Name.Equals(x.Command));
            var admin = permissions.Where(x => x.Command.Equals("*")).FirstOrDefault(x => context.Member.Roles.Select(r => r.Name).Contains(x.RoleName));

            if (notYetRegulated || specificallyAllowed != null || admin != null)
            {
                return true;
            }
            return false;
        }

        public static IEnumerable<Permission> GetPermissions(DiscordGuild guild)
        {
            Expression<Func<Permission, bool>> filter = (permission) => permission.Guild.Equals(guild.ToString());
            var permissions = _databaseConnection.GetAll<Permission>(filter);
            return permissions;
        }
    }
}