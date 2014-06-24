/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 21.06.2014
 * Time: 22:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;

namespace SubmittedData
{
    /// <summary>
    /// Description of ScoringSystem.
    /// </summary>
    public class ScoringSystem
    {
        IResults _user;
        IResults _actual;
		
        public ScoringSystem(IResults user, IResults actual)
        {
            _user = user;
            _actual = actual;
        }
        
        public int GetTotal()
        {
            int score = 0;
            score += GetStageOneMatchScore();
            score += GetQualifierScore();
            return score;
        }

        public int GetStageOneMatchScore()
        {
            int score = 0;
            for (int i = 0; i < _user.GetStageOne().results.Length; i++)
            {
                for (int j = 0; j< _user.GetStageOne().results[i].Length; j++)
                {
                    var actual = _actual.GetStageOne().results[i][j].ToLower();
                    if (actual != "-" && _user.GetStageOne().results[i][j].ToLower() == actual)
                        score++;
                }
            }
            return score;
        }

		public int GetQualifierScore()
		{
		    int score = 0;
		    for (int i = 0; i < _user.GetStageOne().winners.Length; i++)
		    {
		        for (int j = 0; j < _user.GetStageOne().winners[i].Length; j++)
		        {
		            var team = _user.GetStageOne().winners[i][j];
		            if (_actual.GetStageOne().winners[i][j] == "-")
		                continue;
		            if (Array.IndexOf(_actual.GetStageOne().winners[i], team) != -1)
		                score += 2;
		            
		            if (team.Equals(_actual.GetStageOne().winners[i][j]))
		                score += 2;
		        }
		        
		    }
		    return score;
		}
    }
}
