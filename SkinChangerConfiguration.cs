using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CyTic.SkinChanger
{
	public class SkinChangerConfiguration : IRocketPluginConfiguration
	{
		public bool allowNormalSkins;
		public bool overrideCustomSkins;

		public string DatabaseAddress;
		public int DatabasePort;
		public string DatabaseName;
		public string DatabaseTableName;
		public string DatabaseUsername;
		public string DatabasePassword;
		
		public void LoadDefaults()
		{
			allowNormalSkins = false;
			overrideCustomSkins = false;

			DatabaseAddress = "localhost";
			DatabasePort = 3306;
			DatabaseName = "unturned";
			DatabaseTableName = "skins";
			DatabaseUsername = "unturned";
			DatabasePassword = "password";
		}
	}
}
