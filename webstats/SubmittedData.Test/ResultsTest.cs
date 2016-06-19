/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 20.06.2014
 * Time: 22:20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using NUnit.Framework;
using Shouldly;
using Toml;

namespace SubmittedData.Test
{
	/// <summary>
	/// Description of ActualResults.
	/// </summary>
	[TestFixture]
	public class ResultsTest
	{
		private Results CreateTournament(string file, string content)
		{
			var fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
			                                    	{ file, new MockFileData(content) }
			                                    });
				
			return new Results(fileSystem);
		}

		[Test]
		public void TestLoad_ShouldReadFile_IfExists()
		{
			string wc2014 = @"name = ""VM 2014 Brasil""";
			var t = CreateTournament(@"data\vm2014-actual.toml", wc2014);
			Should.NotThrow(() => t.Load(@"data\vm2014-actual.toml"));
		}
		
		[Test]
		public void TestLoad_ShouldFail_IfFileDoesNotExists()
		{
			string wc2014 = @"name = ""VM 2014 Brasil""";
			var t = CreateTournament(@"data\vm2014-actual.toml", wc2014);
			Should.Throw<FileNotFoundException>(() => t.Load(@"configs\vm2014-actual.toml"));
		}

		[Test]
		public void TestIsStageOneComplete()
		{
			string complete = @"[info]
user = ""user1""
[stage-one]
results = [ [ ""h"", ""h"", ""u"", ""b"", ""b"", ""b"",], [ ""b"", ""h"", ""b"", ""b"", ""b"", ""h"",], [ ""h"", ""h"", ""h"", ""u"", ""b"", ""h"",], [ ""b"", ""b"", ""h"", ""b"", ""b"", ""u"",], [ ""h"", ""h"", ""b"", ""b"", ""b"", ""u"",], [ ""h"", ""u"", ""h"", ""h"", ""b"", ""h"",], [ ""h"", ""b"", ""u"", ""u"", ""b"", ""h"",], [ ""h"", ""u"", ""h"", ""b"", ""b"", ""u"",],]
winners = [ [ ""Brasil"", ""Mexico"",], [ ""Spania"", ""Nederland"",], ]
";
			var res = new Results(complete.ParseAsToml());
			res.IsStageOneComplete().ShouldBe(true);

			string incomplete = @"[info]
user = ""user1""
[stage-one]
results = [ [ ""h"", ""h"", ""u"", ""b"", ""b"", ""b"",], [ ""b"", ""h"", ""b"", ""b"", ""b"", ""h"",], [ ""h"", ""h"", ""h"", ""u"", ""b"", ""h"",], [ ""b"", ""b"", ""h"", ""b"", ""b"", ""u"",], [ ""h"", ""h"", ""b"", ""b"", ""-"", ""-"",], [ ""h"", ""u"", ""h"", ""h"", ""b"", ""h"",], [ ""h"", ""b"", ""u"", ""u"", ""b"", ""h"",], [ ""h"", ""u"", ""h"", ""b"", ""b"", ""u"",],]
winners = [ [ ""Brasil"", ""Mexico"",], [ ""Spania"", ""Nederland"",], ]
";
			res = new Results(incomplete.ParseAsToml());
			res.IsStageOneComplete().ShouldBe(false);
		}

		[Test]
		public void TestGetThirdPlaces_ShouldReturnList()
		{
			var r = new Results(@"[info]
user=""foo1""
[stage-one]
results = [ [ ""h"", ""h"", ""h"", ""u"", ""b"", ""-"",], [ ""h"", ""u"", ""h"", ""b"", ""-"", ""-"",], ]
winners = [ [ ""Brasil"", ""Mexico"",], [ ""Spania"", ""Nederland"",], ]
third-places = [ ""Turkey"", ""Switzerland"", ""Italy"", ""Ukraine"",]
".ParseAsToml());
			Assert.That(r.GetThirdPlaces().Length, Is.EqualTo(4));
		}

		[Test]
		public void TestHasThirdPlaces_ShouldReflectContent()
		{
			var r = new Results(@"[info]
user=""foo1""
[stage-one]
results = [ [ ""h"", ""h"", ""h"", ""u"", ""b"", ""-"",], [ ""h"", ""u"", ""h"", ""b"", ""-"", ""-"",], ]
winners = [ [ ""Brasil"", ""Mexico"",], [ ""Spania"", ""Nederland"",], ]
third-places = [ ""Turkey"", ""Switzerland"", ""Italy"", ""Ukraine"",]
".ParseAsToml());

			r.HasThirdPlaces().ShouldBe(true);

			r = new Results(@"[info]
user=""foo1""
[stage-one]
results = [ [ ""h"", ""h"", ""h"", ""u"", ""b"", ""-"",], [ ""h"", ""u"", ""h"", ""b"", ""-"", ""-"",], ]
winners = [ [ ""Brasil"", ""Mexico"",], [ ""Spania"", ""Nederland"",], ]
".ParseAsToml());
			r.HasThirdPlaces().ShouldBe(false);
		}

		[Test]
		public void TestHasBronseFinal_ShouldReflectContent()
		{
			var r = new Results(@"[finals]
bronse-final = ""Brasil""
".ParseAsToml());
			r.HasBronseFinal().ShouldBe(true);

			r = new Results(@"[finals]
".ParseAsToml());
			r.HasBronseFinal().ShouldBe(false);
		}

		[Test]
		public void TestHasFinal_ShouldReflectContent()
		{
			var r = new Results(@"[finals]
final = ""Brasil""
".ParseAsToml());
			r.HasFinal().ShouldBe(true);

			r = new Results(@"[finals]
".ParseAsToml());
			r.HasFinal().ShouldBe(false);
		}
	}
}
