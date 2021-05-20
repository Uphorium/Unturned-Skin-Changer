using Rocket.API.Collections;
using Rocket.API.Serialisation;
using Rocket.API;
using Rocket.Unturned.Player;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Steamworks;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using SDG.Provider;

namespace CyTic.SkinChanger
{
	public class SkinChanger : RocketPlugin<SkinChangerConfiguration>
	{
		public static SkinChanger Instance;
		public static DatabaseManager Database;

		protected override void Load()
		{
			Instance = this;
			Database = new DatabaseManager();
			Provider.onEnemyConnected += playerConnectedSkinHandler;
			Logger.Log("Successfully Loaded!");
		}

		public string skinsToString(int[] skins)
		{
			List<string> skinsStringList = new List<string>();
			skins.All(s => { skinsStringList.Add(s.ToString()); return true; });
			string skinsString = string.Join(",", skinsStringList.ToArray());
			return skinsString;
		}

		public int[] stringToSkins(string skinsString)
		{
			string[] skins = skinsString.Split(',');
			List<int> skinsIntList = new List<int>();
			skins.All(s => { Int32.TryParse(s, out int result); skinsIntList.Add(result); return true; });
			int[] skinsInt = skinsIntList.ToArray();
			return skinsInt;
		}

		public void playerConnectedSkinHandler(SteamPlayer steamPlayer)
		{
			UnturnedPlayer player = UnturnedPlayer.FromSteamPlayer(steamPlayer);
			Database.checkPlayerExists(steamPlayer.playerID.steamID);

			int[] skins = steamPlayer.skinItems;
			int[] oldSkins = steamPlayer.skinItems;

			//Database.SetSkins("76561198068776297", new int[] { 730, 85 });

			if (player.HasPermission("skins.custom"))
			{
				Logger.Log(steamPlayer.playerID.ToString() + " has permission to have custom skins ^-^");

				if (Configuration.Instance.allowNormalSkins)
				{
					Logger.Log("in normal skins");
					if (Configuration.Instance.overrideCustomSkins)
					{
						// normal skins override custom ones
						Logger.Log("normal skins override custom ones");
					}
					else
					{
						// custom skins override normal ones
						Logger.Log("custom skins override normal ones");
						int[] tempSkins = Database.GetSkins(steamPlayer.playerID.ToString());
						List<int[]> skinKeyValue = new List<int[]>();
						//skinKeyValue.Add(new int[]{ 0, 0 });
						ushort item_id;
						ushort item_skin;

						foreach (int skin in oldSkins)
						{
							getItemInfo(skin, out item_id, out item_skin);
							skinKeyValue.Add(new int[] { item_id, item_skin });
						}

						foreach (int skin in tempSkins)
						{
							
						}
					}
				}
				else
				{
					// custom skins only
					//getItemInfo(skins.First(), out result, out result2);
					Logger.Log("custom skins only");
					skins = Database.GetSkins(steamPlayer.playerID.ToString());
				}
			}
			else if (Configuration.Instance.allowNormalSkins)
			{
				// no modifying skins
				Logger.Log(steamPlayer.playerID.ToString() + " hasn't got permissions for custom skins");
				return;
			}
			else
			{
				// no skins given
				skins = new int[0];
				Logger.Log(steamPlayer.playerID.ToString() + " hasn't got permissions for normal skins");
			}
			steamPlayer.skinItems = skins;
		}

		public void setSkin(int itemdefid)
		{
			this.setSkin(itemdefid, false);
		}
		public void setSkin(int defid, bool overrideAll)
		{
			
		}

		public void getItemInfo(int item, out ushort item_id, out ushort item_skin)
		{
			// item is the itemDefID
			UnturnedEconInfo unturnedEconInfo = TempSteamworksEconomy.econInfo.Find((UnturnedEconInfo x) => x.itemdefid == item);
			if (unturnedEconInfo == null)
			{
				item_id = 0;
				item_skin = 0;
				//vehicle_id = 0;
				//return 0;
			}
			item_id = (ushort)unturnedEconInfo.item_id;
			item_skin = (ushort)unturnedEconInfo.item_skin;
			//vehicle_id = (ushort)unturnedEconInfo.vehicle_id;

			//return (ushort)unturnedEconInfo.item_id;
		}

		public override TranslationList DefaultTranslations => new TranslationList()
		{
			{"command_skins_help", "Gives you info about skins"}
		};
	}
}
