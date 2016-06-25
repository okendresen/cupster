/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 15.06.2014
 * Time: 22:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Shouldly;
using SubmittedData;


namespace Modules.Test
{
	/// <summary>
    /// Description of IndexPageViewModelTests.
	/// </summary>
	[TestFixture]
	public class IndexPageViewModelTests
	{
		[Ignore]
		[Test]
		public void TestPageTitle_ShouldReturnTournamentName()
		{
			var tmock = new Mock<ITournament>();
			tmock.Setup(t=> t.GetName())
				.Returns("VM 2014 Brasil");
			
            var bmock = new Mock<ISubmittedBets>();
            bmock.Setup(f => f.GetBetters()).Returns(new List<string>());
			var amock = new Mock<IResultCollection>();

			var groups = new IndexPageViewModel(tmock.Object, bmock.Object, amock.Object);
			groups.PageTitle.ShouldBe("VM 2014 Brasil");
		}

		[Ignore]
		[Test]
		public void TestTournament_ShouldReturnTournamentName()
		{
			var tmock = new Mock<ITournament>();
			tmock.Setup(t=> t.GetName())
				.Returns("VM 2014 Brasil");
			
            var bmock = new Mock<ISubmittedBets>();
            bmock.Setup(f => f.GetBetters()).Returns(new List<string>());
			var amock = new Mock<IResultCollection>();
			amock.Setup(f => f.Current).Returns(new Results());
			var rc = new ResultCollection();
			rc.Current = new Results();

			var groups = new IndexPageViewModel(tmock.Object, bmock.Object, rc);
			groups.Tournament.ShouldBe("VM 2014 Brasil");
		}
	}
}
