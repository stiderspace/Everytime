﻿using Smod2;
using Smod2.API;
using Smod2.Attributes;
using Smod2.Config;
using Smod2.EventHandlers;
using Smod2.Events;
using Smod2.Lang;
using Smod2.Piping;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace EveryTime
{
	[PluginDetails(
		author = "stiderspace",
		name = "EveryTime",
		description = "Loggs admin Times ",
		id = "space.stider.EveryTime",
		configPrefix = "et",
		langFile = "exampleplugin",
		version = "1.0",
		SmodMajor = 3,
		SmodMinor = 4,
		SmodRevision = 0
		)]
	public class EveryTime: Plugin
	{
        [ConfigOption("rank_list")]
        public readonly string[] rankList = {};

        [ConfigOption("ignore_id")]
        public readonly string[] ignoredIds = {};

        [ConfigOption("log_file_name")]
        public readonly string logFileName = "Everytime_log.xml";

        [ConfigOption("log_location_global")]
        public readonly bool logLocation = false;

        [ConfigOption("remove_nonranked_player")]
        public readonly bool removeNonRank = true;


        private string logDirectory;
        public string logFileLocation { get; private set; }

        public List<PlayerData> onlinePlayerList { get; set; }

        

        public override void OnDisable()
        {
            this.Info(this.Details.name + " was disabled");
        }

        public override void OnEnable()
        {

            logDirectory = FileManager.GetAppFolder(GetConfigBool("et_log_location_global"), true, false, false) + "EveryTime/";
            logFileLocation = logDirectory + GetConfigString("et_log_file_name");
            this.Debug(logFileLocation);

            SetupLogFiles();

            if(!SteamManager.Running)
            {
                SteamManager.StartClient(); 
            }
            //this.Info("Steammanager is running:" + SteamManager.Running);
            this.Error("HELLO THERE IM HERE TO TAKE OVER EVERYTHING");
            
        }

        public override void Register()
        {
            
            this.Debug("setting things up here i go");
            this.AddEventHandlers(new EventHandler(this));

                                 
            this.AddCommand("getPlayTime", new GetPlayTimeCommand(this)); // gets the playtime of a user and current minut played if online
            this.AddCommand("getList", new GetListCommand(this)); // gets the list of total playtimes by player
            this.AddCommand("getRankList", new GetRankListCommand(this)); //gets the playtime by rank
            this.AddCommand("getTop", new GetTopCommand(this)); //gets the top 10 players of the server


            this.AddCommand("ResetPlayTime", new ResetPlayTimeCommand(this)); //resets uses playtime
            this.AddCommand("removePlayTime", new RemovePlayTimeCommand(this)); //removes user from playtime log
            
        }

        private void SetupLogFiles()
        {

            this.Debug("Setting up my log files");
            this.Debug(logFileLocation);
            this.Debug(logDirectory);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            if (!File.Exists(logFileLocation))
            {
                XDocument logFile = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("this document contains all the Time data of the admins on the server. PLEASE DO NOT EDIT THIS FILE!!!!"),
                    new XElement("Users"));
                logFile.Save(logFileLocation);
            }
        }

    }
}