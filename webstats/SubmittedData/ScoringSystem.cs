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

        public int GetStageOneMatchScore()
        {
            int score = 0;
            for (int i = 0; i < _user.GetStageOne().results.Length; i++)
            {
                for (int j = 0; j< _user.GetStageOne().results[i].Length; j++)
                {
                    if (_user.GetStageOne().results[i][j].ToLower() == _actual.GetStageOne().results[i][j].ToLower())
                        score++;
                }
            }
            return score;
        }
    }
}
