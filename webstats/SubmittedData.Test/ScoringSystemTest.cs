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
		    s.GetStageOneMatchScore().ShouldBe(2*6);
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
		public void TestGetStageOneMatchScore_ShoulNotCountScore_WhenResultIsDash()
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
		    s.GetStageOneMatchScore().ShouldBe(5+4);
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
		    s.GetQualifierScore().ShouldBe(2);

		    string twoCorrect = @"[info]
user = ""user1""
[stage-one]
winners = [ [ ""Mexico"", ""Brasil"",], ]
";
		    user = new Results(twoCorrect.ParseAsToml());
		    s = new ScoringSystem(user, actual);
		    s.GetQualifierScore().ShouldBe(4);		    
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
		    var user = new Results(oneCorrect.ParseAsToml());
		    var actual = new Results(actualbet.ParseAsToml());
		    var s = new ScoringSystem(user, actual);
		    s.GetQualifierScore().ShouldBe(4);

		    string twoCorrect = @"[info]
user = ""user1""
[stage-one]
winners = [ [ ""Brasil"", ""Mexico"",], ]
";
		    user = new Results(twoCorrect.ParseAsToml());
		    s = new ScoringSystem(user, actual);
		    s.GetQualifierScore().ShouldBe(8);
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
		public void TestGetQualifierScore_ShoulNotdReturnPoints_WhenActualResultsIsDash()
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
		public void TestGetRound16Score_ShouldReturn8PointsPerCorrectWinner()
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
		    s.GetRound16Score().ShouldBe(8);

		    res = @"[stage-two]
round-of-16 = [ ""Brasil"", ""Spania"", ]
";		                  
		    actual = new Results(res.ParseAsToml());
		    s = new ScoringSystem(user, actual);
		    s.GetRound16Score().ShouldBe(16);

		    res = @"[stage-two]
round-of-16 = [ ""NotATeam"", ""Spania"", ]
";		                  
		    actual = new Results(res.ParseAsToml());
		    s = new ScoringSystem(user, actual);
		    s.GetRound16Score().ShouldBe(8);
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
		    s.GetRound16Score().ShouldBe(16);
		}

		[Test]
		public void TestGetRound16Score_ShouldNotReturnPoints_WhenActualIsDash()
		{
		    string res = @"[stage-two]
round-of-16 = [ ""Tyskland"", ""-"", ]
";		                  
		    var actual = new Results(res.ParseAsToml());
		    var s = new ScoringSystem(actual, actual);
		    s.GetRound16Score().ShouldBe(8);
		}
		
		[Test]
		public void TestGetQuarterFinalScore_ShouldReturn16PointsPerCorrectWinner()
		{
		    string bet = @"[stage-two]
quarter-final = [ ""Brasil"", ""Spania"", ""Tyskland"", ""Argentina"",]
";
		    string res = @"[stage-two]
quarter-final = [ ""Brasil"",]
";		                  
		    var user = new Results(bet.ParseAsToml());
		    var actual = new Results(res.ParseAsToml());
		    var s = new ScoringSystem(user, actual);
		    s.GetQuarterFinalScore().ShouldBe(16);

		    res = @"[stage-two]
quarter-final = [ ""Brasil"", ""Spania"", ]
";		                  
		    actual = new Results(res.ParseAsToml());
		    s = new ScoringSystem(user, actual);
		    s.GetQuarterFinalScore().ShouldBe(16+16);

		    res = @"[stage-two]
quarter-final = [ ""NotATeam"", ""Spania"", ]
";		                  
		    actual = new Results(res.ParseAsToml());
		    s = new ScoringSystem(user, actual);
		    s.GetQuarterFinalScore().ShouldBe(16);
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
		    s.GetQuarterFinalScore().ShouldBe(16+16);
		}

		[Test]
		public void TestGetQuarterFinalScore_ShouldNotReturnPoints_WhenActualIsDash()
		{
		    string res = @"[stage-two]
quarter-final = [ ""Tyskland"", ""-"", ]
";		                  
		    var actual = new Results(res.ParseAsToml());
		    var s = new ScoringSystem(actual, actual);
		    s.GetQuarterFinalScore().ShouldBe(16);
		}
		
		[Test]
		public void TestGetSemiFinalScore_ShouldReturn32PointsPerCorrectWinner()
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
		    s.GetSemiFinalScore().ShouldBe(32+32);
		}

		[Test]
		public void TestGetBronseFinalScore_ShouldReturn16PointsForCorrectWinner()
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
		    s.GetBronseFinalScore().ShouldBe(16);
		}

		[Test]
		public void TestGetFinalScore_ShouldReturn32PointsForCorrectWinner()
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
		    s.GetFinalScore().ShouldBe(32);
		}
	}
}
