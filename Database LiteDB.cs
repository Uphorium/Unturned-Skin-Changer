using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDB;
using Rocket.API;
using Rocket.Unturned.Player;
using Steamworks;
using Rocket.Core.Logging;
using System.Text;
using Logger = Rocket.Core.Logging.Logger;
using MySql.Data.MySqlClient;

namespace CyTic.SkinChanger
{
	public class DatabaseManager
	{
		public string Path;
		public LiteCollection<SkinCollection> skinsCollection;

		internal DatabaseManager()
		{
			Path = Directory.GetCurrentDirectory() + "\\Databases\\CyTic.SkinChanger.db";
			bool flag = !Directory.Exists(Directory.GetCurrentDirectory() + "\\Databases");
			if (flag)
			{
				Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Databases");
			}
			LiteDatabase liteDatabase = new LiteDatabase(Path, null);
			skinsCollection = liteDatabase.GetCollection<SkinCollection>("PlayerSkins");
			Logger.Log("LiteDB loaded.", ConsoleColor.Green);
		}

		public void checkPlayerExists(CSteamID steamID)
		{
			bool flag = skinsCollection.FindOne((SkinCollection c) => c.Id == steamID.m_SteamID) != null;
			if (!flag)
			{
				SkinCollection skinCollection = new SkinCollection
				{
					Id = steamID.m_SteamID,
					Skins = new List<SingleSkin>()
				};
				skinsCollection.Insert(skinCollection);
			}
		}
		
		public void addSkin()
		{

		}

        internal int[] GetSkins(string v)
        {
            throw new NotImplementedException();
        }

        internal MySqlConnection createConnection()
        {
            throw new NotImplementedException();
        }
    }
}
