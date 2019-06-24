using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace EveryTime
{
    class EventHandler : IEventHandler, IEventHandlerPlayerJoin, IEventHandlerLateDisconnect
    {
        private readonly EveryTime plugin;


        public EventHandler(EveryTime plugin)
        {
            this.plugin = plugin;
            plugin.onlinePlayerList = new List<PlayerData> { };
        }


        public void OnPlayerJoin(PlayerJoinEvent ev)
        {
            Player player = ev.Player;
            if (plugin.onlinePlayerList != null)
            {
                if (ev.Player != null)
                {
                    if (plugin.rankList.Contains(player.GetUserGroup().Name))
                    {
                        plugin.Debug("User:" + player.Name + " with steamId: " + player.SteamId + ", is logged");
                        plugin.onlinePlayerList.Add(new PlayerData(ev.Player, DateTime.Now));
                    }
                    else if (plugin.GetConfigBool("et_remove_nonranked_player") && LogHelper.userHasData(player.SteamId, plugin))
                    {
                        removeUser(player.SteamId);
                        plugin.Debug("User:" + player.Name + " with steamId: " + player.SteamId + ", is no longer a logged player, his data is removed");
                    }
                }
                else
                {
                    plugin.Debug("there was an oepsie the players data is NULL, im not loggin this player right now");
                }
            }
            else
            {
                plugin.Debug("there was an oepsie. My online playerlist is NULL, im not loggin this player right now");
            }

        }

        public void OnLateDisconnect(LateDisconnectEvent ev)
        {
            List<PlayerData> playerlist = plugin.onlinePlayerList.ToList();
            List<Player> playersOnline = plugin.Server.GetPlayers();
            foreach (PlayerData player in playerlist)
            {
                plugin.Debug("players is disconecting " + ev.Connection.IpAddress);
                plugin.Debug("ip found in list: " + player.player.IpAddress);
                if (!playersOnline.Contains(player.player))
                {
                    plugin.Info("player " + player.player.Name + " is logging out");
                    save(DateTime.Now, player);
                    plugin.onlinePlayerList.Remove(player);
                }
            }
        }


        private void saveExistingUser(DateTime logoutTime, PlayerData player)
        {
            XDocument logFile = XDocument.Load(plugin.logFileLocation);
            XElement user = logFile.Element("Users").Elements("User").Where(x => x.Attribute("steamId").Value == player.player.SteamId).FirstOrDefault();


            int playTime = Convert.ToInt32(Math.Round((logoutTime - player.loginTime).TotalMinutes));
            int totalTime = int.Parse(user.Element("TotalTime").Value);
            int timeThisWeek = int.Parse(user.Element("TotalTimeWeek").Value);
            DateTime lastLogin = DateTime.Parse(user.Element("LastLogin").Value);



            totalTime = totalTime + playTime;

            if (checkweek(player.loginTime, logoutTime))
            { timeThisWeek = timeThisWeek + playTime; }
            else
            { timeThisWeek = playTime; }

            user.SetElementValue("Username", player.player.Name);
            user.SetElementValue("Rank", player.player.GetUserGroup().Name);
            user.SetElementValue("TotalTime", totalTime);
            user.SetElementValue("TotalTimeWeek", timeThisWeek);
            user.SetElementValue("LastLogin", lastLogin);
            SaveFile(logFile);
        }
        private void saveNewUser(DateTime logoutTime, PlayerData player)
        {
            XDocument logFile = XDocument.Load(plugin.logFileLocation);
            int playTime = Convert.ToInt32(Math.Round((logoutTime - player.loginTime).TotalMinutes));

            logFile.Element("Users").Add(
                new XElement("User", new XAttribute("steamId", player.player.SteamId),
                new XElement("Username", player.player.Name),
                new XElement("Rank", player.player.GetUserGroup().Name),
                new XElement("TotalTime", playTime),
                new XElement("TotalTimeWeek", playTime),
                new XElement("LastLogin", player.loginTime)));
            SaveFile(logFile);
        }

        private void SaveFile(XDocument logFile)
        {
            logFile.Save(plugin.logFileLocation);
        }

        public void save(DateTime logoutTime, PlayerData player)
        {
            if (LogHelper.userHasData(player.player.SteamId, plugin))
            {
                saveExistingUser(logoutTime, player);
            }
            else
            {
                saveNewUser(logoutTime, player);
            }
        }

        private bool checkweek(DateTime lastLogin, DateTime currentlogin)
        {
            CultureInfo myCI = new CultureInfo("en-US");
            Calendar cal = myCI.Calendar;

            int weekLast = cal.GetWeekOfYear(lastLogin, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            int weekNow = cal.GetWeekOfYear(currentlogin, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);

            return weekLast == weekNow;

        }

        public void removeUser(string steamId)
        {
            XDocument logFile = XDocument.Load(plugin.logFileLocation);
            logFile.Element("Users").Elements("User").Where(x => x.Attribute("steamId").Value == steamId).FirstOrDefault().Remove();
            SaveFile(logFile);
        }
    }
}