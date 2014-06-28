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
            score += GetRound16Score();
            score += GetQuarterFinalScore();
            score += GetSemiFinalScore();
            score += GetBronseFinalScore();
            score += GetFinalScore();
            return score;
        }

        // 1 point per correct match outcome (win/loss/draw)
        public int GetStageOneMatchScore()
        {
            int score = 0;
            for (int i = 0; i < _user.GetStageOne().results.Length; i++)
            {
                for (int j = 0; j < _user.GetStageOne().results[i].Length; j++)
                {
                    var actual = _actual.GetStageOne().results[i][j].ToLower();
                    if (actual != "-" && _user.GetStageOne().results[i][j].ToLower() == actual)
                        score++;
                }
            }
            return score;
        }

        // 2 points per correct qualifier
        // 2 points per correct position (winner/runner-up)
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

        // 8 points per correct winner
        public int GetRound16Score()
        {
            int score = 0;
            foreach (var team in _user.GetRound16Winners())
            {
                if (team != "-" && Array.IndexOf(_actual.GetRound16Winners(), team) != -1)
                    score += 8;
            }
            return score;
        }

        // 16 points per correct winner
        public int GetQuarterFinalScore()
        {
            int score = 0;
            if (_actual.HasQuarterFinals())
            {
                foreach (var team in _user.GetQuarterFinalWinners())
                {
                    if (team != "-" && Array.IndexOf(_actual.GetQuarterFinalWinners(), team) != -1)
                        score += 16;
                }
            }
            return score;
        }

        // 32 points per correct winner
        public int GetSemiFinalScore()
        {
            int score = 0;
            if (_actual.HasSemiFinals())
            {
                foreach (var team in _user.GetSemiFinalWinners())
                {
                    if (team != "-" && Array.IndexOf(_actual.GetSemiFinalWinners(), team) != -1)
                        score += 32;
                }
            }
            return score;
        }

        // 16 points for correct winner
        public int GetBronseFinalScore()
        {
            int score = 0;
            if (_actual.HasBronseFinal() && _actual.GetBronseFinalWinner() != "-"
                && (_user.GetBronseFinalWinner() == _actual.GetBronseFinalWinner()))
                score += 16;
            return score;
        }

        // 32 points for correct winner
        public int GetFinalScore()
        {
            int score = 0;
            if (_actual.HasFinal() &&_actual.GetFinalWinner() != "-"
                && (_user.GetFinalWinner() == _actual.GetFinalWinner()))
                score += 32;
            return score;
        }
    }
}
