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
using NUnit.Framework;
using Shouldly;

namespace SubmittedData.Test
{
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
		
		[Test]
		public void TestLoadAll_ShouldFail_IfFolderDoesNotExist()
		{
			var fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
			                                    	{ @"data\vm2014-person1.toml", new MockFileData("foo") },
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
			                                    	{ @"data\vm2014-person1.toml", new MockFileData("foo=\"foo\"") },
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
			                                    	{ @"data\vm2014-person1.toml", new MockFileData("foo") },
			                                    });
			var bets = new SubmittedBets(fileSystem);
			bool success = bets.LoadAll(@"data\vm2014-person1.toml");
			success.ShouldBe(false);
		}

		[Test]
		public void TestCount_ShouldReturnNumberOfFilesRead()
		{
			var fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
			                                    	{ @"data\vm2014-person1.toml", new MockFileData("foo=\"foo\"") },
			                                    	{ @"data\vm2014-person2.toml", new MockFileData("foo=\"foo\"") },
			                                    	{ @"data\vm2014-person3.toml", new MockFileData("foo=\"foo\"") },
			                                    	{ @"data\vm2014-person4.toml", new MockFileData("foo=\"foo\"") },
			                                    	{ @"data\vm2014-person5.toml", new MockFileData("foo=\"foo\"") }
			                                    });
			var bets = new SubmittedBets(fileSystem);
			bets.LoadAll("data");
			bets.Count.ShouldBe(5);

			fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
			                                    	{ @"data\vm2014-person1.toml", new MockFileData("foo=\"foo\"") },
			                                    	{ @"data\vm2014-person4.toml", new MockFileData("foo=\"foo\"") },
			                                    	{ @"data\vm2014-person5.toml", new MockFileData("foo=\"foo\"") }
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
			                                    	{ @"data\vm2014-person1.toml", new MockFileData("foo=\"foo\"") },
			                                    	{ @"data\vm2014-person2.toml", new MockFileData("foo=\"foo\"") },
			                                    	{ @"data\vm2014-person3.toml", new MockFileData("foo=\"foo\"") },
			                                    	{ @"data\vm2014.toml", new MockFileData("foo=\"foo\"") },
			                                    	{ @"data\vm2014-person5.toml", new MockFileData("foo=\"foo\"") }
			                                    });
			var bets = new SubmittedBets(fileSystem);
			bets.TournamentFile = @"data\vm2014.toml";
			bets.LoadAll("data");
			bets.Count.ShouldBe(4);
		}
		
		[Test]
		public void TestGetBetters_ShouldReturnNamesOfAllBetters()
		{
			var fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
			                                    	{ @"data\vm2014-person1.toml", new MockFileData("[info]\nuser = \"person1\"") },
			                                    	{ @"data\vm2014-person4.toml", new MockFileData("[info]\nuser = \"person2\"") },
			                                    	{ @"data\vm2014-person5.toml", new MockFileData("[info]\nuser = \"person3\"") }
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
			var fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
			                                    	{ @"data\vm2014-person1.toml", new MockFileData(full) },
			                                    });
			var bets = new SubmittedBets(fileSystem);
			bets.LoadAll("data");
			var betters = bets.GetBetters();
			betters.Count.ShouldBe(1);
			betters.ShouldContain("AGiailoglou");
		}
	}
}
