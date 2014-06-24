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
		    var user = new SubmittedBets.UserResults(full.ParseAsToml());
		    var actual = new SubmittedBets.UserResults(full.ParseAsToml());
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
		    var user = new SubmittedBets.UserResults(userbet.ParseAsToml());
		    var actual = new SubmittedBets.UserResults(actualbet.ParseAsToml());
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
		    var user = new SubmittedBets.UserResults(full.ParseAsToml());
		    var actual = new SubmittedBets.UserResults(full.ParseAsToml());
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
		    var user = new SubmittedBets.UserResults(oneCorrect.ParseAsToml());
		    var actual = new SubmittedBets.UserResults(actualbet.ParseAsToml());
		    var s = new ScoringSystem(user, actual);
		    s.GetQualifierScore().ShouldBe(2);

		    string twoCorrect = @"[info]
user = ""user1""
[stage-one]
winners = [ [ ""Mexico"", ""Brasil"",], ]
";
		    user = new SubmittedBets.UserResults(twoCorrect.ParseAsToml());
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
		    var user = new SubmittedBets.UserResults(oneCorrect.ParseAsToml());
		    var actual = new SubmittedBets.UserResults(actualbet.ParseAsToml());
		    var s = new ScoringSystem(user, actual);
		    s.GetQualifierScore().ShouldBe(4);

		    string twoCorrect = @"[info]
user = ""user1""
[stage-one]
winners = [ [ ""Brasil"", ""Mexico"",], ]
";
		    user = new SubmittedBets.UserResults(twoCorrect.ParseAsToml());
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
		    var user = new SubmittedBets.UserResults(oneCorrect.ParseAsToml());
		    var actual = new SubmittedBets.UserResults(actualbet.ParseAsToml());
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
		    var actual = new SubmittedBets.UserResults(actualbet.ParseAsToml());
		    var s = new ScoringSystem(actual, actual);
		    s.GetQualifierScore().ShouldBe(0);
		}
	}
}
