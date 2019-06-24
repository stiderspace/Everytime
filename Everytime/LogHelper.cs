using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EveryTime
{
    public class LogHelper
    {
        public static bool userHasData(string steamId, EveryTime plugin)
        {
            XDocument logFile = XDocument.Load(plugin.logFileLocation);
            return logFile.Element("Users").Elements("User").Attributes("steamId").Any(att => att.Value == steamId);
        }

        public static string getUsername(ulong steamId)
        {
            string username = steamId.ToString();
            if (SteamManager.Running)
            {
                username = SteamManager.GetPersonaName(steamId);
            }
            
            return username;
            

        }

    }
}
