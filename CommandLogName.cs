using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;
using Rocket.Unturned.Player;
using System.IO;
using SDG.Unturned;
using Rocket.Unturned.Chat;
using Steamworks;

namespace NameChanger
{
    class CommandLogName : IRocketCommand
    {
        public List<string> Aliases
        {
            get { return new List<string> { "logn" }; }
        }

        public AllowedCaller AllowedCaller
        {
            get { return Rocket.API.AllowedCaller.Both; }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length != 1)
            {
                msg(caller, "Invalid usage, correct: /logn <player> or /logn all");
                return;
            }

            if (command[0].ToLower() == "all")
            {
                WriteToFile((CSteamID)0, true, true, caller);
            }
            else
            {
                UnturnedPlayer player = UnturnedPlayer.FromName(command[0]);
                if (player == null)
                {
                    msg(caller, "Player not found!");
                    return;
                }

                WriteToFile(player.CSteamID, false, true, caller);
            }
        }

        public string Help
        {
            get { return "logs a persons display name"; }
        }

        public string Name
        {
            get { return "logname"; }
        }

        public List<string> Permissions
        {
            get { return new List<string>(); }
        }

        public string Syntax
        {
            get { return "<player>"; }
        }

        void WriteToFile(CSteamID ID, bool allNames, bool delete, IRocketPlayer caller)
        {
            if (delete)
            {
                if (File.Exists(Environment.CurrentDirectory + @"\NameLog.txt"))
                {
                    File.Delete("NameLog.txt");
                }
            }

            if (!File.Exists(Environment.CurrentDirectory + @"\" + getFileName()))
            {
                var createdFile = File.Create("NameLog.txt");
                createdFile.Close();
            }

            UnturnedPlayer uPlayer = null;
            string names = "";
            if (allNames)
            {
                foreach (var player in Provider.Players)
                {
                    uPlayer = UnturnedPlayer.FromSteamPlayer(player);
                    names += string.Format("\n CSteamID: [{0}] DisplayName: [{1}]  SteamProfileUrl: [{2} ] SteamName: [{3}]",
                        uPlayer.CSteamID.ToString(),
                        uPlayer.DisplayName,
                        String.Format("https://www.steamcommunity.com/id/{0}", uPlayer.SteamProfile.CustomURL),
                        uPlayer.SteamName);
                }

                var writer = File.AppendText(getFileName());
                writer.Write(names);
                writer.Close();
                return;
            }

            if (ID.ToString() != "0")
            {
                var targetPlayer = UnturnedPlayer.FromCSteamID(ID);
                names += string.Format("\n CSteamID: [{0}] DisplayName: [{1}]  SteamProfileUrl: [{2} ] SteamName: [{3}]",
                           targetPlayer.CSteamID,
                           targetPlayer.DisplayName,
                           String.Format("https://steamcommunity.com/profiles/{0}", targetPlayer.CSteamID),
                           targetPlayer.SteamName);

                if (targetPlayer != null)
                {
                    var writer = File.AppendText(getFileName());
                    writer.Write(names);
                    writer.Close();
                }
                else
                {
                    msg(caller, "Player not found!");
                }
            }
        }

        string getFileName()
        {
            return "NameLog.txt";
        }

        void msg(IRocketPlayer caller, string message)
        {
            if (caller is ConsolePlayer)
            {
                NameChanger.Log(message);
            }
            else
            {
                UnturnedChat.Say(caller, message);
            }
        }
    }
}
