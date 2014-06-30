/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 30.06.2014
 * Time: 19:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;
using Shouldly;
using Toml;

namespace SubmittedData.Test
{
    /// <summary>
    /// Description of AchievementSystemTest.
    /// </summary>
    [TestFixture]
    public class AchievementSystemTest
    {
        [Test]
        [Ignore]
        public void TestGetPerfectGroup_ReturnsTrue_WhenAllGroupMatchesAndBothQualifiersAreCorrect()
        {
		    string userBet = @"[stage-one]
winners = [ [ ""Germany"", ""Algerie"",], ]
";
		    string actualResults = @"[stage-one]
winners = [ [ ""Brasil"", ""Mexico"",], ]
";
		    var user = new Results(userBet.ParseAsToml());
		    var actual = new Results(actualResults.ParseAsToml());

		    var a = new AchievementSystem(user, actual);
            bool p = a.GetPerfectGroup();
        }
        
        [Test]
        public void TestHasDoubleRainbow_ReturnsFalse_WhenNoGroupWithBothQualifiers()
        {
		    string userBet = @"[stage-one]
winners = [ [ ""Germany"", ""Algerie"",], ]
";
		    string actualResults = @"[stage-one]
winners = [ [ ""Brasil"", ""Mexico"",], ]
";
		    var user = new Results(userBet.ParseAsToml());
		    var actual = new Results(actualResults.ParseAsToml());

		    var a = new AchievementSystem(user, actual);
            a.HasDoubleRainbow().ShouldBe(false);
        }

        [Test]
        public void TestHasDoubleRainbow_ReturnsTrue_WhenAtLeastOneGroupWithBothQualifiers()
        {
		    string userBet = @"[stage-one]
winners = [ [ ""Brasil"", ""Algerie"",], [ ""Spania"", ""Nederland"",], ]
";
		    string actualResults = @"[stage-one]
winners = [ [ ""Brasil"", ""Mexico"",], [ ""Spania"", ""Nederland"",], ]
";
		    var user = new Results(userBet.ParseAsToml());
		    var actual = new Results(actualResults.ParseAsToml());

		    var a = new AchievementSystem(user, actual);
		    a.HasDoubleRainbow().ShouldBe(true);
        }
    }
}
