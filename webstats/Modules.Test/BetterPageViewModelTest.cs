/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 19.06.2014
 * Time: 22:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Moq;
using NUnit.Framework;
using Shouldly;
using SubmittedData;
using Toml;

namespace Modules.Test
{
	/// <summary>
	/// Description of BetterViewModelTest.
	/// </summary>
	[TestFixture]
	public class BetterPageViewModelTest
	{
		[Test]
		public void TestBetter_ShouldReturnBettersName()
		{
			var tmock = new Mock<ITournament>();
			var amock = new Mock<IResults>();
			dynamic bet = "[info]\nuser=\"foo1\"".ParseAsToml();
			var better = new BetterPageViewModel(tmock.Object, bet, amock.Object);
			better.Better.ShouldBe("foo1");
		}
		
		[Test]
		public void TestPageTitle_ShouldReturBettersName()
		{
			var tmock = new Mock<ITournament>();
			var amock = new Mock<IResults>();
			dynamic bet = "[info]\nuser=\"foo1\"".ParseAsToml();
			var better = new BetterPageViewModel(tmock.Object, bet, amock.Object);
			better.PageTitle.ShouldBe("foo1");
		}
		
		[Test]
		public void TestGroupMatches_GetResult_ShouldReturnTeam1_WhenResultIsWin()
		{
			var gm = new BetterPageViewModel.GroupMatches();
			var match = new Tuple<string, string, string, string>("team1", "team2", "h", "h");
			gm.GetResults(match).ShouldBe("team1");
			
			var match2 = new Tuple<string, string, string, string>("toto", "fofo", "h", "h");
			gm.GetResults(match2).ShouldBe("toto");
		}

		[Test]
		public void TestGroupMatches_GetResult_ShouldReturnTeam2_WhenResultIsLoss()
		{
			var gm = new BetterPageViewModel.GroupMatches();
			var match = new Tuple<string, string, string, string>("team1", "team2", "b", "b");
			gm.GetResults(match).ShouldBe("team2");

			var match2 = new Tuple<string, string, string, string>("toto", "fofo", "b", "b");
			gm.GetResults(match2).ShouldBe("fofo");
		}

		[Test]
		public void TestGroupMatches_GetResult_ShouldReturnDraw_WhenResultIsDraw()
		{
			var gm = new BetterPageViewModel.GroupMatches();
			var match = new Tuple<string, string, string, string>("team1", "team2", "u", "u");
			gm.GetResults(match).ShouldBe("Draw");

			var match2 = new Tuple<string, string, string, string>("toto", "fofo", "u", "u");
			gm.GetResults(match2).ShouldBe("Draw");
		}

		[Test]
		public void TestGroupMatches_GetResult_ShouldBeCaseInsensitve()
		{
			var gm = new BetterPageViewModel.GroupMatches();
			var match = new Tuple<string, string, string, string>("team1", "team2", "h", "h");
			var match2 = new Tuple<string, string, string, string>("team1", "team2", "H", "H");
			gm.GetResults(match).ShouldBe(gm.GetResults(match2));

			match = new Tuple<string, string, string, string>("team1", "team2", "b", "b");
			match2 = new Tuple<string, string, string, string>("team1", "team2", "B", "B");
			gm.GetResults(match).ShouldBe(gm.GetResults(match2));

			match = new Tuple<string, string, string, string>("team1", "team2", "u", "u");
			match2 = new Tuple<string, string, string, string>("team1", "team2", "U", "U");
			gm.GetResults(match).ShouldBe(gm.GetResults(match2));
		}
		
		[Test]
		public void TestGrouMatches_GetResults_ShouldReturnEmpty_WhenNoResults()
		{
			var gm = new BetterPageViewModel.GroupMatches();
			var match = new Tuple<string, string, string, string>("team1", "team2", "h", "-");
			gm.GetResults(match, "-").ShouldBe("");
		}
	}
}
