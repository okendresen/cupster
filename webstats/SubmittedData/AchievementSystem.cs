/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 30.06.2014
 * Time: 19:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace SubmittedData
{
    public enum AchievementTypes
    {
        DoubleRainbow,
        NotEvenClose,
    }
        
    /// <summary>
    /// Description of AchievementSystem.
    /// </summary>
    public class AchievementSystem
    {
        IResults _user;
        IResults _actual;
		
        public AchievementSystem(dynamic user, dynamic actual)
        {
            _user = user;
            _actual = actual;
            CreateAchievements();
            CheckDoubleRainbow();
        }

        List<Achievement> _achievements = new List<Achievement>();
        public List<Achievement> Achievements { get { return _achievements; } }
        
        Dictionary<AchievementTypes, Achievement> _achievementsRepo = new Dictionary<AchievementTypes, Achievement>();
        public Dictionary<AchievementTypes, Achievement> AchievementRepo
        {
            get
            {
                return _achievementsRepo;
            }
            private set
            {
                _achievementsRepo = value;
            }
        }
        
        void CheckDoubleRainbow()
        {
            for (int i = 0; i < _user.GetStageOne().winners.Length; i++)
            {
                if ((_actual.GetStageOne().winners[i][0].Equals(_user.GetStageOne().winners[i][0]))
                    && (_actual.GetStageOne().winners[i][1].Equals(_user.GetStageOne().winners[i][1])))
                {
                    _achievements.Add(_achievementsRepo[AchievementTypes.DoubleRainbow]);
                    break;
                }
		                 
            }
        }

        void CheckPerfectGroup()
        {
        }

        void CreateAchievements()
        {
            _achievementsRepo.Add(AchievementTypes.DoubleRainbow, 
                new Achievement() {
                    Image = "double-rainbow", 
                    Title = "Double rainbow: Both qualifiers correct in at least one group"
                });
            
        }
		
		
        public class Achievement
        {
            public string Image { get; set; }
            public string Title { get; set; }
        }
    }
}
