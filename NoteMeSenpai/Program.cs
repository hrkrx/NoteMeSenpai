using System;
using System.IO;
using System.Collections.Generic;

namespace NoteMeSenpai
{
    class Program
    {
        static void Main(string[] args)
        {
            string apiKey = "<BOT-API-SECRET>";

            if (File.Exists("api.key")) apiKey = File.ReadAllText("api.key");

            DiscordBot.Start(apiKey).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
