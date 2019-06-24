using System;
using System.Linq;
using System.Xml.Linq;
using Smod2.API;
using Smod2.Commands;

namespace EveryTime
{
    internal class ResetPlayTimeCommand : ICommandHandler
    {
        private EveryTime plugin;

        public ResetPlayTimeCommand(EveryTime plugin)
        {
            this.plugin = plugin;
        }

        public string GetCommandDescription()
        {
            return "Resets the Playtime of a user setting his total and weekly to O";
        }

        public string GetUsage()
        {
            return "RESETPLAYTIME {StreamId}";
        }

        public string[] OnCall(ICommandSender sender, string[] args)
        {
            if(args.Length == 1)
            {
                if (sender is Player player)
                {
                    if (!player.HasPermission("everytime.reset"))
                    {
                        return new[] { "You dont have permission to reset a players playtime." };
                    }
                }

                resetUser(args[0]);
                return new[] { "user play time has been reset" };                         

            }

            return new[] { " missing Arguments. USAGE: RESETPLAYTIME {StreamId}" };

        }

        private void resetUser(string steamId)
        {
            XDocument logFile = XDocument.Load(plugin.logFileLocation);
            XElement user = logFile.Element("Users").Elements("User").Where(x => x.Attribute("steamId").Value == steamId).FirstOrDefault();
            user.SetElementValue("TotalTime", 0);
            user.SetElementValue("TotalTimeWeek", 0);
        }
    }
}