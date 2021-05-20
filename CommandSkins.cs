using Rocket.API;
using Rocket.Unturned.Player;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.Unturned.Chat;
using MySql.Data.MySqlClient;
using Rocket.Core;
using Steamworks;

namespace CyTic.SkinChanger
{
	public class CommandSkins : IRocketCommand
	{
		public string Name
		{
			get
			{
				return "skins";
			}
		}
		public string Help
		{
			get
			{
				string message = SkinChanger.Instance.Translate("command_skins_help");
				return message;
			}
		}
		public string Syntax
		{
			get
			{
				return "";
			}
		}
		public List<string> Aliases
		{
			get { return new List<string>(); }
		}
		public List<string> Permissions
		{
			get
			{
				return new List<string>() { "skins" };
			}
		}

		public AllowedCaller AllowedCaller
		{
			get
			{
				return AllowedCaller.Both;
			}
		}

		public void Execute(IRocketPlayer caller, string[] command)
		{
			UnturnedPlayer player = (UnturnedPlayer)caller;
			UnturnedChat.Say(player, "you can use skins! ^-^", new UnityEngine.Color(255,255,25,25));
		}
	}

	public class CommandTest : IRocketCommand
	{
		public string Name
		{
			get
			{
				return "test";
			}
		}
		public string Help
		{
			get
			{
				string message = "testing command";
				return message;
			}
		}
		public string Syntax
		{
			get
			{
				return "";
			}
		}
		public List<string> Aliases
		{
			get { return new List<string>(); }
		}

		public List<string> Permissions
		{
			get
			{
				return new List<string>() { };
			}
		}

		public AllowedCaller AllowedCaller
		{
			get
			{
				return AllowedCaller.Both;
			}
		}

		public void Execute(IRocketPlayer caller, string[] command)
		{
			UnturnedPlayer player = (UnturnedPlayer)caller;
			player.Player.equipment.applySkinVisual();
			player.Player.equipment.applyMythicVisual();
			player.Player.equipment.updateState();
		}


		/*Provider.clients[0].skinItems = new int[] { 698 };
		Provider.clients[0].player.equipment.updateState();
		Provider.clients[0].player.equipment.applySkinVisual();
		Provider.clients[0].player.equipment.applyMythicVisual();
		//player.Player.equipment.applySkinVisual();
		*/
	}

	public class CommandTestb : IRocketCommand
	{
		public string Name
		{
			get
			{
				return "testb";
			}
		}
		public string Help
		{
			get
			{
				string message = "testing command";
				return message;
			}
		}
		public string Syntax
		{
			get
			{
				return "";
			}
		}
		public List<string> Aliases
		{
			get
			{
				return new List<string>() { };
			}
		}
		public List<string> Permissions
		{
			get
			{
				return new List<string>() { };
			}
		}

		public AllowedCaller AllowedCaller
		{
			get
			{
				return AllowedCaller.Both;
			}
		}

		public void Execute(IRocketPlayer caller, string[] commandText)
		{
			//UnturnedPlayer player = (UnturnedPlayer)caller;

			if (commandText.Length == 0)
			{
				try
				{
					MySqlConnection connection = SkinChanger.Database.createConnection();
					MySqlCommand command = connection.CreateCommand();
					command.CommandText = "select `skins` from `" + SkinChanger.Instance.Configuration.Instance.DatabaseTableName + "` where `steamId` = '" + 76561198068776297.ToString() + "';";
					Logger.Log(command.CommandText);
					connection.Open();
					object result = command.ExecuteScalar();
					Logger.Log(result.ToString());
				}
				catch (Exception ex)
				{
					Logger.LogException(ex);
				}
			}
			else if (commandText[0] == "f")
			{
				int[] skins = SkinChanger.Database.GetSkins("76561198068776297");

			}
			else
			{
				try
				{
					MySqlConnection connection = SkinChanger.Database.createConnection();
					MySqlCommand command = connection.CreateCommand();
					command.CommandText = commandText[0];
					Logger.Log(command.CommandText);
					connection.Open();
					object result = command.ExecuteScalar();
					Logger.Log(result.ToString());
				}
				catch (Exception ex)
				{
					Logger.LogException(ex);
				}
			}
		}
	}

	public class CommandTesta : IRocketCommand
	{
		public string Name
		{
			get
			{
				return "testa";
			}
		}
		public string Help
		{
			get
			{
				string message = "testing command";
				return message;
			}
		}
		public string Syntax
		{
			get
			{
				return "";
			}
		}
		public List<string> Aliases
		{
			get
			{
				return new List<string>() { };
			}
		}
		public List<string> Permissions
		{
			get
			{
				return new List<string>() { };
			}
		}

		public AllowedCaller AllowedCaller
		{
			get
			{
				return AllowedCaller.Player;
			}
		}

		public void Execute(IRocketPlayer caller, string[] command)
		{
			UnturnedPlayer player = (UnturnedPlayer)caller;
			string test;
			Logger.Log(test = SkinChanger.Instance.skinsToString(Provider.clients.Find(c => c.playerID.steamID == player.CSteamID).skinItems));
			Logger.Log(SkinChanger.Instance.stringToSkins(test).ToString());

			for (int i = 0; i < Provider.clients.Count; i++)
			{
				if (Provider.clients[i].playerID.steamID == player.CSteamID)
				{
					//string test;
					test = "";
					Logger.Log(test = SkinChanger.Instance.skinsToString(Provider.clients[i].skinItems));
					Logger.Log(SkinChanger.Instance.stringToSkins(test).ToString());
				}
			}
		}


		/*Provider.clients[0].skinItems = new int[] { 698 };
		Provider.clients[0].player.equipment.updateState();
		Provider.clients[0].player.equipment.applySkinVisual();
		Provider.clients[0].player.equipment.applyMythicVisual();
		//player.Player.equipment.applySkinVisual();
		*/
	}
}
