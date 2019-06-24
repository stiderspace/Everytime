using Smod2.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace EveryTime
{
    internal class GetListCommand : ICommandHandler
    {
        private EveryTime plugin;

        public GetListCommand(EveryTime plugin)
        {
            this.plugin = plugin;
        }

        public string GetCommandDescription()
        {
            return "Returns the list of play times of the server ";
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

            if (args[0].ToLower().Equals("total"))
            {
                return getTotalTop(users).ToArray();
            }
            else if (args[0].ToLower().Equals("week"))
            {
                return getWeekTop(users).ToArray();
            }
            else
            {
                return new[] { "illigal argument: must be \"WEEK\" or \"TOTAL\" " };
            }

        }

        private List<string> getTotalTop(IEnumerable<XElement> users)
        {
            List<string> data = new List<string>();
            List<XElement> sorted = users.OrderByDescending(o => o.Element("TotalTime").Value).ToList();

            data.Add("----------[Play time of players on the server]----------");
            foreach(XElement user in sorted)
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

        private List<string> getWeekTop(IEnumerable<XElement> users)
        {
            List<string> data = new List<string>();
            List<XElement> sorted = users.OrderByDescending(o => o.Element("TotalTime").Value).ToList();

            data.Add("----------[Play time of players of this week]----------");
            foreach (XElement user in sorted)
            {
                int totalTime = int.Parse(user.Element("TotalTimeWeek").Value);
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