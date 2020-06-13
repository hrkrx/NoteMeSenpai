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
            var prefixes = new List<string>() {"-"};

            if (File.Exists("api.key")) apiKey = File.ReadAllText("api.key");
            
            DiscordBot.Start(apiKey, prefixes).ConfigureAwait(false).GetAwaiter().GetResult();;
        }
    }
}
