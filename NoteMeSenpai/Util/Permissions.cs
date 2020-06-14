using System;
using System.Linq;
using NoteMeSenpai.Models;
using System.Linq.Expressions;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using NoteMeSenpai.Database;

namespace NoteMeSenpai.Util
{
    public static class Permissions
    {

        private static DatabaseConnection _databaseConnection;

        public static void Init(DatabaseConnection dbConn)
        {
            _databaseConnection = dbConn;
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

            Expression<Func<Permission, bool>> filter = (permission) => permission.Guild.Equals(context.Guild.ToString());
            var permissions = _databaseConnection.GetAll<Permission>(filter);
            var notYetRegulated = permissions.Where(x => x.Command.Equals(context.Command.Name) || x.Command.Equals("*")).Count() == 0;
            var specificallyAllowed = permissions.FirstOrDefault(x => context.Member.Roles.Select(r => r.Name).Contains(x.RoleName)  && context.Command.Name.Equals(x.Command));
            var admin = permissions.FirstOrDefault(x => context.Member.Roles.Select(r => r.Name).Contains(x.RoleName)  && context.Command.Name.Equals(x.Command)&& (context.Command.Name.Equals(x.Command) || x.Command.Equals("*")));

            if (notYetRegulated || specificallyAllowed != null || admin != null)
            {
                return true;
            }
            return false;
        }
    }
}