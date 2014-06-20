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
	public class BetterViewModelTest
	{
		[Test]
		public void TestBetter_ShouldReturnBettersName()
		{
			var tmock = new Mock<ITournament>();
			dynamic bet = "[info]\nuser=\"foo1\"".ParseAsToml();
			var better = new BetterViewModel(tmock.Object, bet);
			better.Better.ShouldBe("foo1");
		}
		
		[Test]
		public void TestPageTitle_ShouldReturBettersName()
		{
			var tmock = new Mock<ITournament>();
			dynamic bet = "[info]\nuser=\"foo1\"".ParseAsToml();
			var better = new BetterViewModel(tmock.Object, bet);
			better.PageTitle.ShouldBe("foo1");
		}
		
		[Test]
		public void TestGroupMatches_GetResult_ShouldReturnTeam1_WhenResultIsWin()
		{
			var gm = new BetterViewModel.GroupMatches();
			var match = new Tuple<string, string, string>("team1", "team2", "h");
			gm.GetResults(match).ShouldBe("team1");
			
			var match2 = new Tuple<string, string, string>("toto", "fofo", "h");
			gm.GetResults(match2).ShouldBe("toto");
		}

		[Test]
		public void TestGroupMatches_GetResult_ShouldReturnTeam2_WhenResultIsLoss()
		{
			var gm = new BetterViewModel.GroupMatches();
			var match = new Tuple<string, string, string>("team1", "team2", "b");
			gm.GetResults(match).ShouldBe("team2");

			var match2 = new Tuple<string, string, string>("toto", "fofo", "b");
			gm.GetResults(match2).ShouldBe("fofo");
		}

		[Test]
		public void TestGroupMatches_GetResult_ShouldReturnDraw_WhenResultIsDraw()
		{
			var gm = new BetterViewModel.GroupMatches();
			var match = new Tuple<string, string, string>("team1", "team2", "u");
			gm.GetResults(match).ShouldBe("Draw");

			var match2 = new Tuple<string, string, string>("toto", "fofo", "u");
			gm.GetResults(match2).ShouldBe("Draw");
		}
	}
}
