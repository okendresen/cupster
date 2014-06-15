/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 15.06.2014
 * Time: 22:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Moq;
using NUnit.Framework;
using Shouldly;
using SubmittedData;


namespace Modules.Test
{
	/// <summary>
	/// Description of GroupsViewModelTests.
	/// </summary>
	[TestFixture]
	public class GroupsViewModelTests
	{
		[Test]
		public void TestTournament_ShouldReturnTournamentName()
		{
			var mock = new Mock<ITournament>();
			mock.Setup(t=> t.GetName())
				.Returns("VM 2014 Brasil");
			
			var groups = new GroupsViewModel(mock.Object);
			groups.Tournament.ShouldBe("VM 2014 Brasil");
		}
	}
}
