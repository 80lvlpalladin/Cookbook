using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Cookbook.Client.Utils
{
    /// <summary>
    /// TODO: summary
    /// </summary>
    public static class GlobalStrings
    {
        public static Uri APIHostAddress
        {
            get
            {
                var jObject = JObject.Parse(
                    File.ReadAllText(BaseDirectory + "launchSettings.json"));

                return new Uri(jObject["APIHostAddress"].Value<string>());
            }
        }

        public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;
    }

    
}

