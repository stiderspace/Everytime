using Smod2.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace EveryTime
{
    internal class GetTopCommand : ICommandHandler
    {
        private EveryTime plugin;

        public GetTopCommand(EveryTime plugin)
        {
            this.plugin = plugin;
        }

        public string GetCommandDescription()
        {
            return "Returns the top 10 play times of the server ";
        }

        public string GetUsage()
        {
            return "PLAYTIMETOP {Total/week}";
        }

        public string[] OnCall(ICommandSender sender, string[] args)
        {
            if (args.Length != 1)
            {
                return new[] { "in valid arguments only 1 argument needed", "USAGE: PLAYTIMETOP {Total/week}" };
            }

            XDocument log = XDocument.Load(plugin.logFileLocation);
            IEnumerable<XElement> users = log.Element("Users").Elements("User");

            if(args[0].ToLower().Equals("total"))
            {
                return getTotalTop(users).ToArray();
            }
            else if (args[0].ToLower().Equals("week"))
            {
                return getWeekTop(users).ToArray();
            }
            else
            {
                return new[] { "illigal argument: must be \"WEEK\" or \"TOTAL\" "};
            }

        }

        private List<string> getTotalTop(IEnumerable<XElement> users)
        {
            List<string> data = new List<string>();
            List<XElement> sorted = users.OrderByDescending(o => o.Element("TotalTime").Value).ToList();

            int top = 10;
            if (sorted.ToArray().Length != top)
            {
                top = sorted.ToArray().Length;
            }

            data.Add("----------[Top 10 Players of the server]----------");
            for (int i = 0; i < top; i++)
            {
                int totalTime = int.Parse(sorted[i].Element("TotalTime").Value);
                //string username = LogHelper.getUsername(ulong.Parse(sorted[i].Attribute("steamId").Value)); 
                string username = sorted[i].Element("Username").Value;
                string userString = String.Format("User: {0} -- ", username);
                string totalTimeString = String.Format("{0} minutes", totalTime);

                data.Add(userString + totalTimeString);
            }
            data.Add("----------------------------------------------");


            return data;
        }

        private List<string> getWeekTop(IEnumerable<XElement> users)
        {
            List<string> data = new List<string>();
            List<XElement> sorted = users.OrderByDescending(o => o.Element("TotalTime").Value).ToList();
            int top = 10;
            if(sorted.ToArray().Length != top)
            {
                top = sorted.ToArray().Length;
            }

            data.Add("----------[Top 10 players of this week]----------");
            for(int i = 0; i < top; i++)
            {
                int totalTime = int.Parse(sorted[i].Element("TotalTimeWeek").Value);
               // string username = LogHelper.getUsername(ulong.Parse(sorted[i].Attribute("steamId").Value));
                string username = sorted[i].Element("Username").Value;
                string userString = String.Format("User: {0} -- ", username);
                string totalTimeString = String.Format("{0} minutes", totalTime);

                data.Add(userString + totalTimeString);
            }
            data.Add("----------------------------------------------");

            return data;
        }
    }
}