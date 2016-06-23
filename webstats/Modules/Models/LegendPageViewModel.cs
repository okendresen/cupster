using System.Collections.Generic;
using SubmittedData;

namespace Modules
{
	class LegendPageViewModel
	{
		AchievementSystem _achievementSystem;

		public LegendPageViewModel(IResults actual)
		{
			_achievementSystem = new AchievementSystem(actual, actual);
		}

		public string PageTitle
		{
			get { return "Achievement legend"; }
		}

		public List<AchievementSystem.Achievement> AchievementLegend
		{
			get
			{
				return _achievementSystem.AchievementList;
			}

		}
	}
}