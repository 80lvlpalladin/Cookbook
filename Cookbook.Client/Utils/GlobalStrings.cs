using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Cookbook.Client.Utils
{
    /// <summary>Application-level strings</summary>
    public static class GlobalStrings
    {
        /// <summary>Parses address set in launchSettings.json</summary>
        public static Uri APIHostAddress
        {
            get
            {
                var jObject = JObject.Parse(
                    File.ReadAllText(BaseDirectory + "launchSettings.json"));

                return new Uri(jObject["APIHostAddress"].Value<string>());
            }
        }

        /// <summary>Directory where .exe is located</summary>
        public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;
    }

    
}

