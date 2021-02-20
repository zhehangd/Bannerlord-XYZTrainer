using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace XYZTrainer
{
	
    public class XYZTrainerState : GameState
	{
		

		private static AtmosphereInfo CreateAtmosphereInfoForMission(string seasonId, int timeOfDay)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add("spring", 0);
			dictionary.Add("summer", 1);
			dictionary.Add("fall", 2);
			dictionary.Add("winter", 3);
			int season = 0;
			dictionary.TryGetValue(seasonId, out season);
			Dictionary<int, string> dictionary2 = new Dictionary<int, string>();
			dictionary2.Add(6, "TOD_06_00_SemiCloudy");
			dictionary2.Add(12, "TOD_12_00_SemiCloudy");
			dictionary2.Add(15, "TOD_04_00_SemiCloudy");
			dictionary2.Add(18, "TOD_03_00_SemiCloudy");
			dictionary2.Add(22, "TOD_01_00_SemiCloudy");
			string atmosphereName = "field_battle";
			dictionary2.TryGetValue(timeOfDay, out atmosphereName);
			return new AtmosphereInfo
			{
				AtmosphereName = atmosphereName,
				TimeInfo = new TimeInformation
				{
					Season = season
				}
			};
		}

		public XYZCombatant PlayerParty { get; set; }
		public XYZCombatant EnemyParty { get; set; }

		public BasicCharacterObject PlayerCharacter { get; set; }

		public BasicCultureObject MainCulture { get; set; }
	}
}