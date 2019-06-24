using Smod2.API;
using Smod2.Commands;
using System;
using System.Linq;
using System.Xml.Linq;

namespace EveryTime
{
    internal class GetPlayTimeCommand : ICommandHandler
    {
        private EveryTime plugin;

        public GetPlayTimeCommand(EveryTime plugin)
        {
            this.plugin = plugin;
        }

        public string GetCommandDescription()
        {
            return "Returns the the play time of a user";
        }

        public string GetUsage()
        {
            return "GETPLAYTIME {SteamId/online Username}";
        }

        public string[] OnCall(ICommandSender sender, string[] args)
        {
            if(args.Length != 1)
            {
                return new[] { "USAGE: GETPLAYTIME {SteamId/online Username}" };
            }
            if(sender is Player player)
            {
                if (!player.HasPermission("everytime.playerTime"))
                {
                    return new[] { "You dont have permission to view a players playtime." };
                }
            }

            foreach(PlayerData playerData in plugin.onlinePlayerList)
            {
                if(playerData.player.Name.Equals(args[0]))
                {
                    return getPlayerData(playerData.player.SteamId);
                }
            }

            return getPlayerData(args[0]);
        }

        private string[] getPlayerData(string steamId)
        {

            XDocument log = XDocument.Load(plugin.logFileLocation);
            if(!LogHelper.userHasData(steamId, plugin))
            {
                return new[] { "User has no player data logged" };
            }
            XElement user = log.Element("Users").Elements("User").Where(x => x.Attribute("steamId").Value == steamId).FirstOrDefault();
            

            int totalTime = int.Parse(user.Element("TotalTime").Value);
            int timeThisWeek = int.Parse(user.Element("TotalTimeWeek").Value);
            DateTime lastLogin = DateTime.Parse(user.Element("LastLogin").Value);
            //string username = LogHelper.getUsername(ulong.Parse(user.Attribute("steamId").Value));
            string username = user.Element("Username").Value;


            string userString = String.Format("User: {0}", username);
            string totalTimeString = String.Format("Total play time: {0}", totalTime);
            string totalweekString = String.Format("Total play time this week: {0}", timeThisWeek);
            string lastLoginString = String.Format("Last login of user: {0}", lastLogin);

            return new[] { userString, lastLoginString, totalTimeString, totalweekString };
        }
    }
}