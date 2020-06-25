using System;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.Generic;

namespace NoteMeSenpai.Models
{
    public class Options {
        public string DatabaseConnectionString { get; set; }
        public int DeletionDelayInSeconds { get; set; }
        public List<string> Prefixes { get; set; }
        
        public static Options LoadFromFile(string path = null)
        {
            if (path == null)
            {
                path = "settings.json";
            }
            Options options = (Options)JsonSerializer.Deserialize(File.ReadAllText(path), typeof(Options));
            return options;
        }

        public static void SaveToFile(Options options, string path = null)
        {
            if (path == null)
            {
                path = "settings.json";
            }

            var jsonString = JsonSerializer.Serialize(options, typeof(Options));
            File.WriteAllText(path, jsonString);
        }
    }
    
}