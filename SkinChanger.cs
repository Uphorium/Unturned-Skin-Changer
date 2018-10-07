using Rocket.API.Collections;
using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CyTic.SkinChanger
{
	public class SkinChanger : RocketPlugin<SkinChangerConfiguration>
	{
		public override TranslationList DefaultTranslations
		{
			get
			{
				return new TranslationList()
				{
					{"TwentyFour_max_players_set", "max_players set to {0} from {1}"}
				};
			}
		}

		protected override void Load()
		{
			
		}

		protected override void Unload()
		{

		}
	}
}
