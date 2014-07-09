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
        CompleteMiss,
        Sweet16,
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
            CheckNotEvenClose();
            CheckCompleteMiss();
            CheckSweet16();
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
            if (_user.HasStageOne())
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
        }

        void CheckNotEvenClose()
        {
            if (_user.HasSemiFinals() && _actual.HasStageTwo())
            {
                bool found = false;
                var finalists = _user.GetSemiFinalWinners();
                foreach (var pair in _actual.GetStageOne().winners)
                {
                    if (Array.IndexOf(pair, finalists[0]) != -1 || Array.IndexOf(pair, finalists[1]) != -1)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    _achievements.Add(_achievementsRepo[AchievementTypes.NotEvenClose]);
                }
            }
        }

        void CheckCompleteMiss()
        {
            if (_user.HasStageOne() && _actual.HasStageOne())
            {
                for (int i = 0; i < _user.GetStageOne().results.Length; i++)
                {
                    bool found = false;
                    for (int j = 0; j < _user.GetStageOne().results[i].Length; j++)
                    {
                        var actual = _actual.GetStageOne().results[i][j].ToLower();
                        if (_user.GetStageOne().results[i][j].ToLower() == actual)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        _achievements.Add(_achievementsRepo[AchievementTypes.CompleteMiss]);
                        break;
                    }
                }		        
            }
        }

        void CheckSweet16()
        {
            if (_user.HasRound16() && _actual.HasRound16())
            {
                bool found = true;
                for (int i = 0; i < _user.GetRound16Winners().Length; i++)
                {
                    if (_user.GetRound16Winners()[i] != _actual.GetRound16Winners()[i])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    _achievements.Add(_achievementsRepo[AchievementTypes.Sweet16]);
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
                    Image = "double-rainbow.png", 
                    Title = "Double rainbow: Both qualifiers correct in at least one group"
                });
            _achievementsRepo.Add(AchievementTypes.NotEvenClose,
                new Achievement() {
                    Image = "not-even.png",
                    Title = "Not even close: Both finalists knocked out in the group round"
                });
            _achievementsRepo.Add(AchievementTypes.CompleteMiss,
                new Achievement() {
                    Image = "complete-miss.jpg",
                    Title = "Complete miss: Group with no correct matches"
                });
            _achievementsRepo.Add(AchievementTypes.Sweet16,
                new Achievement() {
                    Image = "sweet16.jpg",
                    Title = "Sweet 16: Every winner of round of 16 matches"
                });
        }
		
		
        public class Achievement
        {
            public string Image { get; set; }
            public string Title { get; set; }
        }
    }
}
