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
        public void TestPerfectGroup_ReturnsTrue_WhenAllGroupMatchesAndBothQualifiersAreCorrect()
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
        }
        
        [Test]
        public void TestAchievements_ShouldNotContainDoubleRainbow_WhenNoGroupWithBothQualifiers()
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
		    a.Achievements.ShouldBeEmpty();
        }

        [Test]
        public void TestAchievements_ShouldContainDoubleRainbow_WhenAtLeastOneGroupWithBothQualifiers()
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
		    a.Achievements.ShouldContain(a.AchievementRepo[AchievementTypes.DoubleRainbow]);
        }
        
        [Test]
        public void TestAchievements_ShouldContainNotEvenClose_WhenBothFinalistsAreKnockedOutInGroup()
        {
		    string userBet = @"[stage-two]
semi-final = [ ""Spania"", ""Italia"",]
";
		    string actualResults = @"[stage-one]
winners = [ [ ""Brasil"", ""Mexico"",], [ ""Nederland"", ""Chile"",], [ ""Colombia"", ""Hellas"",], ]
[stage-two]
";
		    var user = new Results(userBet.ParseAsToml());
		    var actual = new Results(actualResults.ParseAsToml());

		    var a = new AchievementSystem(user, actual);
		    a.Achievements.ShouldContain(a.AchievementRepo[AchievementTypes.NotEvenClose]);
        }

        [Test]
        public void TestAchievements_ShouldContainCompleteMiss_WhenGroupwithNoCorrectMatches()
        {
		    string userBet = @"[info]
user = ""user1""
[stage-one]
results = [ [ ""H"", ""B"", ""H"", ""H"", ""B"", ""H"",], [ ""B"", ""B"", ""H"", ""B"", ""B"", ""H"",], [ ""U"", ""B"", ""U"", ""H"", ""H"", ""U"",], [ ""B"", ""B"", ""B"", ""H"", ""H"", ""B"",], [ ""B"", ""H"", ""B"", ""B"", ""U"", ""B"",], [ ""H"", ""U"", ""H"", ""U"", ""B"", ""H"",], [ ""H"", ""B"", ""H"", ""B"", ""b"", ""h"",], [ ""b"", ""h"", ""b"", ""u"", ""u"", ""u"",],]
winners = [ [ ""Brasil"", ""Mexico"",], [ ""Spania"", ""Nederland"",], ]
";
		    string actualResults = @"[info]
user = ""user1""
[stage-one]
results = [ [ ""h"", ""h"", ""u"", ""b"", ""b"", ""b"",], [ ""b"", ""h"", ""b"", ""b"", ""b"", ""h"",], [ ""h"", ""h"", ""h"", ""u"", ""b"", ""h"",], [ ""b"", ""b"", ""h"", ""b"", ""b"", ""u"",], [ ""h"", ""h"", ""b"", ""b"", ""b"", ""u"",], [ ""h"", ""u"", ""h"", ""h"", ""b"", ""h"",], [ ""h"", ""b"", ""u"", ""u"", ""b"", ""h"",], [ ""h"", ""u"", ""h"", ""b"", ""b"", ""u"",],]
winners = [ [ ""Brasil"", ""Mexico"",], [ ""Spania"", ""Nederland"",], ]
";
		    var user = new Results(userBet.ParseAsToml());
		    var actual = new Results(actualResults.ParseAsToml());

		    var a = new AchievementSystem(user, actual);
		    a.Achievements.ShouldContain(a.AchievementRepo[AchievementTypes.CompleteMiss]);
        }
    }
}
