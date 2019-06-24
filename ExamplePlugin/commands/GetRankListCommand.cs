using Smod2.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace EveryTime
{
    internal class GetRankListCommand : ICommandHandler
    {
        private EveryTime plugin;

        public GetRankListCommand(EveryTime plugin)
        {
            this.plugin = plugin;
        }

        public string GetCommandDescription()
        {
            return "Returns the list of play times of the server sorted by ranks";
        }

        public string GetUsage()
        {
            return "GETRANKLIST {Total/week}";
        }

        public string[] OnCall(ICommandSender sender, string[] args)
        {
            plugin.Info("||" + args[0] +"||");
            if (args.Length != 1)
            {
                return new[] { "in valid arguments only 1 argument needed", "USAGE: GETRANKLIST {Total/Week}" };
            }
            if (!(args[0].ToLower() == "week"))
            {
                
                return new[] { "illigal argument: must be \"WEEK\" or \"TOTAL\" " };
            }
            if (!(args[0].ToLower() != "total"))
            {
                return new[] { "illigal argument: must be \"WEEK\" or \"TOTAL\" " };
            }

            XDocument log = XDocument.Load(plugin.logFileLocation);
            List<XElement> users = log.Element("Users").Elements("User").ToList();

            List<string> dataAll = new List<string>();

            
            for (int i = 0; i < plugin.rankList.Length; i++)
            {
                List<XElement> rankUsers = users.Where(u => u.Element("Rank").Value == plugin.rankList[i]).ToList();
                if(args[0].ToLower().Equals("week"))
                {
                    dataAll.AddRange(RankDataTotal(rankUsers, plugin.rankList[i]));
                }
                else
                {
                    dataAll.AddRange(RankDataWeek(rankUsers, plugin.rankList[i]));
                }
            }
            return dataAll.ToArray();
        }

        private List<string> RankDataTotal(List<XElement> users, string rank)
        {
            List<string> data = new List<string>();
            
            List<XElement> sorted = users.OrderByDescending(o => o.Element("TotalTime").Value).ToList();


            data.Add("----------[Play time of the "+ rank + "]----------");
            foreach (XElement user in sorted)
            {
                int totalTime = int.Parse(user.Element("TotalTime").Value);
                //string username = LogHelper.getUsername(ulong.Parse(user.Attribute("steamId").Value));
                string username = user.Element("Username").Value;
                string userString = String.Format("User: {0} -- ", username);
                string totalTimeString = String.Format("{0} minutes", totalTime);

                data.Add(userString + totalTimeString);
            }
            data.Add("----------------------------------------------");

            return data;
        }

        private List<string> RankDataWeek(List<XElement> users, string rank)
        {
            List<string> data = new List<string>();

            List<XElement> sorted = users.OrderByDescending(o => o.Element("TotalTime").Value).ToList();


            data.Add("----------[Play time of the " + rank + "]----------");
            foreach (XElement user in sorted)
            {
                int totalTime = int.Parse(user.Element("TotalTime").Value);
                //string username = LogHelper.getUsername(ulong.Parse(user.Attribute("steamId").Value));
                string username = user.Element("Username").Value;
                string userString = String.Format("User: {0} -- ", username);
                string totalTimeString = String.Format("{0} minutes", totalTime);

                data.Add(userString + totalTimeString);
            }
            data.Add("----------------------------------------------");
            return data;
        }
    }
}