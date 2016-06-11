/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 14.06.2014
 * Time: 15:39
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Shouldly;

namespace SubmittedData.Test
{
	internal static class XFS
	{
		internal static string Path(string path, Func<bool> isUnixF = null)
		{
			var isUnix = isUnixF ?? IsUnixPlatform;

			if (isUnix())
			{
				path = Regex.Replace(path, @"^[a-zA-Z]:(?<path>.*)$", "${path}");
				path = path.Replace(@"\", "/");
			}

			return path;
		}

		internal static string Separator(Func<bool> isUnixF = null)
		{
			var isUnix = isUnixF ?? IsUnixPlatform;
			return isUnix() ? "/" : @"\";
		}

		internal static bool IsUnixPlatform()
		{
			int p = (int)Environment.OSVersion.Platform;
			return (p == 4) || (p == 6) || (p == 128);
		}
	}

	/// <summary>
	/// Description of SubmittedBetsTest.
	/// </summary>
	[TestFixture]
	public class SubmittedBetsTest
	{
		private SubmittedBets CreateSubmittedBets(string file, string content)
		{
			var fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
			                                    	{ file, new MockFileData(content) }
			                                    });
				
			return new SubmittedBets(fileSystem);
		}

		private SubmittedBets CreateCompleteBet()
		{
			string full = @"[stage-two]
round-of-16 = [ ""Brasil"", ""Spania"", ""Hellas"", ""Italia"", ""Frankrike"", ""Argentina"", ""Portugal"", ""Tyskland"",]
quarter-final = [ ""Brasil"", ""Italia"", ""Frankrike"", ""Tyskland"",]
semi-final = [ ""Brasil"", ""Italia"",]
[finals]
bronse-final = ""Frankrike""
final = ""Brasil""
[info]
user = ""AGiailoglou""
[stage-one]
results = [ [ ""h"", ""h"", ""h"", ""u"", ""b"", ""b"",], [ ""h"", ""u"", ""h"", ""b"", ""b"", ""h"",], [ ""b"", ""b"", ""h"", ""u"", ""h"", ""h"",], [ ""h"", ""b"", ""u"", ""h"", ""h"", ""b"",], [ ""h"", ""h"", ""b"", ""u"", ""u"", ""b"",], [ ""h"", ""b"", ""h"", ""u"", ""b"", ""u"",], [ ""b"", ""b"", ""h"", ""b"", ""u"", ""h"",], [ ""h"", ""h"", ""u"", ""b"", ""b"", ""b"",],]
winners = [ [ ""Brasil"", ""Mexico"",], [ ""Spania"", ""Nederland"",], [ ""Hellas"", ""Japan"",], [ ""Italia"", ""Uruguay"",], [ ""Frankrike"", ""Sveits"",], [ ""Argentina"", ""Nigeria"",], [ ""Portugal"", ""Tyskland"",], [ ""Belgia"", ""Russland"",],]
";
			var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> { {
					XFS.Path(@"data\vm2014-person1.toml"),
					new MockFileData(full)
				},
			});
			return new SubmittedBets(fileSystem);
		}
		
		[Test]
		public void TestLoadAll_ShouldFail_IfFolderDoesNotExist()
		{
			var fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
			                                    	{ XFS.Path(@"data\vm2014-person1.toml"), new MockFileData("foo") },
			                                    });
			var bets = new SubmittedBets(fileSystem);
			bool success = bets.LoadAll("dont-exist");
			success.ShouldBe(false);
		}

		[Test]
		public void TestLoadAll_ShouldSucceed_IfFolderIsExist()
		{
			var fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
			                                    	{ XFS.Path(@"data\vm2014-person1.toml"), new MockFileData("[info]\nuser=\"foo\"") },
			                                    });
			var bets = new SubmittedBets(fileSystem);
			bool success = bets.LoadAll("data");
			success.ShouldBe(true);
		}

		[Test]
		public void TestLoadAll_ShouldFail_IfNotAFolder()
		{
			var fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
			                                    	{ XFS.Path(@"data\vm2014-person1.toml"), new MockFileData("foo") },
			                                    });
			var bets = new SubmittedBets(fileSystem);
			bool success = bets.LoadAll(XFS.Path(@"data\vm2014-person1.toml"));
			success.ShouldBe(false);
		}

		[Test]
		public void TestCount_ShouldReturnNumberOfFilesRead()
		{
			var fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
													{ XFS.Path(@"data\vm2014-person1.toml"), new MockFileData("[info]\nuser=\"foo1\"") },
													{ XFS.Path(@"data\vm2014-person2.toml"), new MockFileData("[info]\nuser=\"foo2\"") },
													{ XFS.Path(@"data\vm2014-person3.toml"), new MockFileData("[info]\nuser=\"foo3\"") },
													{ XFS.Path(@"data\vm2014-person4.toml"), new MockFileData("[info]\nuser=\"foo4\"") },
													{ XFS.Path(@"data\vm2014-person5.toml"), new MockFileData("[info]\nuser=\"foo5\"") }
			                                    });
			var bets = new SubmittedBets(fileSystem);
			bets.LoadAll("data");
			bets.Count.ShouldBe(5);

			fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
													{ XFS.Path(@"data\vm2014-person1.toml"), new MockFileData("[info]\nuser=\"foo1\"") },
													{ XFS.Path(@"data\vm2014-person4.toml"), new MockFileData("[info]\nuser=\"foo4\"") },
													{ XFS.Path(@"data\vm2014-person5.toml"), new MockFileData("[info]\nuser=\"foo5\"") }
			                                    });
			bets = new SubmittedBets(fileSystem);
			bets.LoadAll("data");
			bets.Count.ShouldBe(3);
		}
		
		[Test]
		public void TestLoadAll_ShouldSkipTournamentConfigFile()
		{
			var fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
			                                    	{ XFS.Path(@"data\vm2014-person1.toml"), new MockFileData("[info]\nuser=\"foo1\"") },
			                                    	{ XFS.Path(@"data\vm2014-person2.toml"), new MockFileData("[info]\nuser=\"foo2\"") },
			                                    	{ XFS.Path(@"data\vm2014-person3.toml"), new MockFileData("[info]\nuser=\"foo3\"") },
			                                    	{ XFS.Path(@"data\vm2014.toml"), new MockFileData("foo=\"foo\"") },
			                                    	{ XFS.Path(@"data\vm2014-person5.toml"), new MockFileData("[info]\nuser=\"foo5\"") }
			                                    });
			var bets = new SubmittedBets(fileSystem);
			bets.TournamentFile = XFS.Path(@"data\vm2014.toml");
			bets.LoadAll("data");
			bets.Count.ShouldBe(4);
		}
		
		[Test]
		public void TestLoadAll_ShouldSkipActualResultsConfigFile()
		{
			var fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
			                                    	{ XFS.Path(@"data\vm2014-person1.toml"), new MockFileData("[info]\nuser=\"foo1\"") },
			                                    	{ XFS.Path(@"data\vm2014-person2.toml"), new MockFileData("[info]\nuser=\"foo2\"") },
			                                    	{ XFS.Path(@"data\vm2014-person3.toml"), new MockFileData("[info]\nuser=\"foo3\"") },
			                                    	{ XFS.Path(@"data\vm2014-actual.toml"), new MockFileData("[info]\nuser=\"actual\"") },
			                                    	{ XFS.Path(@"data\vm2014-person5.toml"), new MockFileData("[info]\nuser=\"foo5\"") }
			                                    });
			var bets = new SubmittedBets(fileSystem);
			bets.ActualResultsFile = XFS.Path(@"data\vm2014-actual.toml");
			bets.LoadAll("data");
			bets.Count.ShouldBe(4);
		}
		
		[Test]
		public void TestGetBetters_ShouldReturnNamesOfAllBetters()
		{
			var fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
													{ XFS.Path(@"data\vm2014-person1.toml"), new MockFileData("[info]\nuser = \"person1\"") },
													{ XFS.Path(@"data\vm2014-person4.toml"), new MockFileData("[info]\nuser = \"person2\"") },
													{ XFS.Path(@"data\vm2014-person5.toml"), new MockFileData("[info]\nuser = \"person3\"") }
			                                    });
			var bets = new SubmittedBets(fileSystem);
			bets.LoadAll("data");
			var betters = bets.GetBetters();
			betters.Count.ShouldBe(3);
			betters.ShouldContain("person1");
			betters.ShouldContain("person2");
			betters.ShouldContain("person3");
		}
		
		[Test]
		public void TestLoadAll_LoadsCompleteFile()
		{
			var bets = CreateCompleteBet();
			bets.LoadAll("data");
			var betters = bets.GetBetters();
			betters.Count.ShouldBe(1);
			betters.ShouldContain("AGiailoglou");
		}
		
		[Test]
		public void TestGetSingleBet_ShouldReturnBet_WhenGivenUserExists()
		{
			var fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
			                                    	{ XFS.Path(@"data\vm2014-person1.toml"), new MockFileData("[info]\nuser=\"foo1\"") },
			                                    	{ XFS.Path(@"data\vm2014-person2.toml"), new MockFileData("[info]\nuser=\"foo2\"") },
			                                    	{ XFS.Path(@"data\vm2014-person3.toml"), new MockFileData("[info]\nuser=\"foo3\"") },
			                                    	{ XFS.Path(@"data\vm2014-person4.toml"), new MockFileData("[info]\nuser=\"foo4\"") },
			                                    	{ XFS.Path(@"data\vm2014-person5.toml"), new MockFileData("[info]\nuser=\"foo5\"") }
			                                    });
			var bets = new SubmittedBets(fileSystem);
			bets.LoadAll("data");
			var bet = bets.GetSingleBet("foo2");
			Assert.That(bet.GetInfo().user, Is.EqualTo("foo2"));
		}
		
		[Test]
		public void TestGetSingleBet_ShouldReturnTypeFullStageOne()
		{
			var bets = CreateCompleteBet();
			bets.LoadAll("data");
			IResults bet = bets.GetSingleBet("AGiailoglou");
			Assert.That(bet.GetStageOne().results.Length, Is.EqualTo(8));
		}
	}
}
