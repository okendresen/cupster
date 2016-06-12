/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 21.06.2014
 * Time: 19:04
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Moq;
using NUnit.Framework;
using Shouldly;
using Sprache;
using Toml;

namespace SubmittedData.Test
{
	/// <summary>
	/// Description of ScoringSystemTest.
	/// </summary>
	[TestFixture]
	public class ScoringSystemTest
	{
		[Test]
		public void TestGetStageOneMatchScore_ShouldReturnFullScore_WhenResultIsSame()
		{
		    string full = @"[info]
user = ""user1""
[stage-one]
results = [ [ ""h"", ""h"", ""h"", ""u"", ""b"", ""b"",], [ ""h"", ""u"", ""h"", ""b"", ""b"", ""h"",], ]
winners = [ [ ""Brasil"", ""Mexico"",], [ ""Spania"", ""Nederland"",], ]
";
		    var user = new Results(full.ParseAsToml());
		    var actual = new Results(full.ParseAsToml());
		    var s = new ScoringSystem(user, actual);
			s.GetStageOneMatchScore().ShouldBe(2*6*ScoringSystem.Points.StageOneMatchOutcome);
		}

		[Test]
		public void TestGetStageOneMatchScore_ShouldReturnZero_WhenAllResultIsDifferent()
		{
		    string userbet = @"[info]
user = ""user1""
[stage-one]
results = [ [ ""h"", ""h"", ""h"", ""u"", ""b"", ""b"",], [ ""h"", ""u"", ""h"", ""b"", ""b"", ""h"",], ]
winners = [ [ ""Brasil"", ""Mexico"",], [ ""Spania"", ""Nederland"",], ]
";
		    string actualbet = @"[info]
user = ""user1""
[stage-one]
results = [ [ ""b"", ""b"", ""b"", ""b"", ""h"", ""h"",], [ ""b"", ""b"", ""b"", ""h"", ""h"", ""b"",], ]
winners = [ [ ""Brasil"", ""Mexico"",], [ ""Spania"", ""Nederland"",], ]
";
		    var user = new Results(userbet.ParseAsToml());
		    var actual = new Results(actualbet.ParseAsToml());
		    var s = new ScoringSystem(user, actual);
		    s.GetStageOneMatchScore().ShouldBe(0);
		}

		[Test]
		public void TestGetStageOneMatchScore_ShouldNotCountScore_WhenResultIsDash()
		{
		    string full = @"[info]
user = ""user1""
[stage-one]
results = [ [ ""h"", ""h"", ""h"", ""u"", ""b"", ""-"",], [ ""h"", ""u"", ""h"", ""b"", ""-"", ""-"",], ]
winners = [ [ ""Brasil"", ""Mexico"",], [ ""Spania"", ""Nederland"",], ]
";
		    var user = new Results(full.ParseAsToml());
		    var actual = new Results(full.ParseAsToml());
		    var s = new ScoringSystem(user, actual);
			s.GetStageOneMatchScore().ShouldBe(5 * ScoringSystem.Points.StageOneMatchOutcome + 
			                                   4 * ScoringSystem.Points.StageOneMatchOutcome);
		}

		[Test]
		public void TestGetQualifierScore_ShouldReturnTwoPointsPerCorrectQualifier()
		{
		    string oneCorrect = @"[info]
user = ""user1""
[stage-one]
winners = [ [ ""Somethingelse"", ""Brasil"",], ]
";
		    string actualbet = @"[info]
user = ""user1""
[stage-one]
winners = [ [ ""Brasil"", ""Mexico"",], ]
";
		    var user = new Results(oneCorrect.ParseAsToml());
		    var actual = new Results(actualbet.ParseAsToml());
		    var s = new ScoringSystem(user, actual);
			s.GetQualifierScore().ShouldBe(ScoringSystem.Points.QualifyingTeam);

		    string twoCorrect = @"[info]
user = ""user1""
[stage-one]
winners = [ [ ""Mexico"", ""Brasil"",], ]
";
		    user = new Results(twoCorrect.ParseAsToml());
		    s = new ScoringSystem(user, actual);
			s.GetQualifierScore().ShouldBe(2 * ScoringSystem.Points.QualifyingTeam);		    
		}
		
		[Test]
		public void TestGetQualifierScore_ShouldReturnFourPointsPerCorrectPlacement()
		{
		    string oneCorrect = @"[info]
user = ""user1""
[stage-one]
winners = [ [ ""Brasil"", ""Whatever"",], ]
";
		    string actualbet = @"[info]
user = ""user1""
[stage-one]
winners = [ [ ""Brasil"", ""Mexico"",], ]
";
			var oneCorrectScore = ScoringSystem.Points.QualifyingTeam + ScoringSystem.Points.QualifyingPosition;
		    var user = new Results(oneCorrect.ParseAsToml());
		    var actual = new Results(actualbet.ParseAsToml());
		    var s = new ScoringSystem(user, actual);
			s.GetQualifierScore().ShouldBe(oneCorrectScore);

		    string twoCorrect = @"[info]
user = ""user1""
[stage-one]
winners = [ [ ""Brasil"", ""Mexico"",], ]
";
		    user = new Results(twoCorrect.ParseAsToml());
		    s = new ScoringSystem(user, actual);
			s.GetQualifierScore().ShouldBe(2*oneCorrectScore);
		}

		[Test]
		public void TestGetQualifierScore_ShoulNotdReturnPoints_WhenGroupIsIncomplete()
		{
		    string oneCorrect = @"[info]
user = ""user1""
[stage-one]
winners = [ [ ""Brasil"", ""Whatever"",], ]
";
		    string actualbet = @"[info]
user = ""user1""
[stage-one]
winners = [ [ ""-"", ""-"",], ]
";
		    var user = new Results(oneCorrect.ParseAsToml());
		    var actual = new Results(actualbet.ParseAsToml());
		    var s = new ScoringSystem(user, actual);
		    s.GetQualifierScore().ShouldBe(0);
		}

		[Test]
		public void TestGetQualifierScore_ShouldNotReturnPoints_WhenActualResultsIsDash()
		{
		    string actualbet = @"[info]
user = ""user1""
[stage-one]
winners = [ [ ""-"", ""-"",], ]
";
		    var actual = new Results(actualbet.ParseAsToml());
		    var s = new ScoringSystem(actual, actual);
		    s.GetQualifierScore().ShouldBe(0);
		}
		
		[Test]
		public void TestGetRound16Score_ShouldReturnPointsPerCorrectWinner()
		{
		    string bet = @"[stage-two]
round-of-16 = [ ""Brasil"", ""Spania"", ""England"", ""Italia"", ""Nigeria"", ""Argentina"", ""Tyskland"", ""Portugal"",]
";
		    string res = @"[stage-two]
round-of-16 = [ ""Brasil"",]
";		                  
		    var user = new Results(bet.ParseAsToml());
		    var actual = new Results(res.ParseAsToml());
		    var s = new ScoringSystem(user, actual);
			s.GetRound16Score().ShouldBe(ScoringSystem.Points.Round16Winner);

		    res = @"[stage-two]
round-of-16 = [ ""Brasil"", ""Spania"", ]
";		                  
		    actual = new Results(res.ParseAsToml());
		    s = new ScoringSystem(user, actual);
			s.GetRound16Score().ShouldBe(2 * ScoringSystem.Points.Round16Winner);

		    res = @"[stage-two]
round-of-16 = [ ""NotATeam"", ""Spania"", ]
";		                  
		    actual = new Results(res.ParseAsToml());
		    s = new ScoringSystem(user, actual);
			s.GetRound16Score().ShouldBe(ScoringSystem.Points.Round16Winner);
		}

		[Test]
		public void TestGetRound16Score_ShouldReturnPointsForWinnersInAnyPosition()
		{
		    string bet = @"[stage-two]
round-of-16 = [ ""Brasil"", ""Spania"", ""England"", ""Italia"", ""Nigeria"", ""Argentina"", ""Tyskland"", ""Portugal"",]
";
		    string res = @"[stage-two]
round-of-16 = [ ""Tyskland"", ""Brasil"", ]
";		                  
		    var user = new Results(bet.ParseAsToml());
		    var actual = new Results(res.ParseAsToml());
		    var s = new ScoringSystem(user, actual);
			s.GetRound16Score().ShouldBe(2 * ScoringSystem.Points.Round16Winner);
		}

		[Test]
		public void TestGetRound16Score_ShouldNotReturnPoints_WhenActualIsDash()
		{
		    string res = @"[stage-two]
round-of-16 = [ ""Tyskland"", ""-"", ]
";		                  
		    var actual = new Results(res.ParseAsToml());
		    var s = new ScoringSystem(actual, actual);
			s.GetRound16Score().ShouldBe(ScoringSystem.Points.Round16Winner);
		}
		
		[Test]
		public void TestGetQuarterFinalScore_ShouldReturnPointsPerCorrectWinner()
		{
			int points = ScoringSystem.Points.QuarterFinalWinner;
		    string bet = @"[stage-two]
quarter-final = [ ""Brasil"", ""Spania"", ""Tyskland"", ""Argentina"",]
";
		    string res = @"[stage-two]
quarter-final = [ ""Brasil"",]
";		                  
		    var user = new Results(bet.ParseAsToml());
		    var actual = new Results(res.ParseAsToml());
		    var s = new ScoringSystem(user, actual);
		    s.GetQuarterFinalScore().ShouldBe(points);

		    res = @"[stage-two]
quarter-final = [ ""Brasil"", ""Spania"", ]
";		                  
		    actual = new Results(res.ParseAsToml());
		    s = new ScoringSystem(user, actual);
		    s.GetQuarterFinalScore().ShouldBe(points + points);

		    res = @"[stage-two]
quarter-final = [ ""NotATeam"", ""Spania"", ]
";		                  
		    actual = new Results(res.ParseAsToml());
		    s = new ScoringSystem(user, actual);
		    s.GetQuarterFinalScore().ShouldBe(points);
		}

		[Test]
		public void TestGetQuarterFinalScore_ShouldReturnPointsForWinnersInAnyPosition()
		{
		    string bet = @"[stage-two]
quarter-final = [ ""Brasil"", ""Spania"", ""Tyskland"", ""Argentina"",]
";
		    string res = @"[stage-two]
quarter-final = [ ""Tyskland"", ""Brasil"", ]
";		                  
		    var user = new Results(bet.ParseAsToml());
		    var actual = new Results(res.ParseAsToml());
		    var s = new ScoringSystem(user, actual);
			s.GetQuarterFinalScore().ShouldBe(2 * ScoringSystem.Points.QuarterFinalWinner);
		}

		[Test]
		public void TestGetQuarterFinalScore_ShouldNotReturnPoints_WhenActualIsDash()
		{
		    string res = @"[stage-two]
quarter-final = [ ""Tyskland"", ""-"", ]
";		                  
		    var actual = new Results(res.ParseAsToml());
		    var s = new ScoringSystem(actual, actual);
			s.GetQuarterFinalScore().ShouldBe(ScoringSystem.Points.QuarterFinalWinner);
		}
		
		[Test]
		public void TestGetSemiFinalScore_ShouldReturnPointsPerCorrectWinner()
		{
		    string bet = @"[stage-two]
semi-final = [ ""Tyskland"", ""Argentina"",]
";
		    string res = @"[stage-two]
semi-final = [ ""Argentina"", ""Tyskland"",]
";		                  
		    var user = new Results(bet.ParseAsToml());
		    var actual = new Results(res.ParseAsToml());
		    var s = new ScoringSystem(user, actual);
			s.GetSemiFinalScore().ShouldBe(2 * ScoringSystem.Points.SemiFinalWinner);
		}

		[Test]
		public void TestGetBronseFinalScore_ShouldReturnPointsForCorrectWinner()
		{
		    string bet = @"[finals]
bronse-final = ""Brasil""
";
		    string res = @"[finals]
bronse-final = ""Brasil""
";		                  
		    var user = new Results(bet.ParseAsToml());
		    var actual = new Results(res.ParseAsToml());
		    var s = new ScoringSystem(user, actual);
			s.GetBronseFinalScore().ShouldBe(ScoringSystem.Points.BronseFinalWinner);
		}

		[Test]
		public void TestGetFinalScore_ShouldReturnPointsForCorrectWinner()
		{
		    string bet = @"[finals]
final = ""Tyskland""
";
		    string res = @"[finals]
final = ""Tyskland""
";		                  
		    var user = new Results(bet.ParseAsToml());
		    var actual = new Results(res.ParseAsToml());
		    var s = new ScoringSystem(user, actual);
			s.GetFinalScore().ShouldBe(ScoringSystem.Points.FinalWinner);
		}
	}
}
