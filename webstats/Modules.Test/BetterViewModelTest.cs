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

	}
}
