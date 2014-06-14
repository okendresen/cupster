/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 12.06.2014
 * Time: 23:39
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using NUnit.Framework;
using Shouldly;

namespace SubmittedData.Test
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	[TestFixture]
	public class TournamentTest
	{
		private Tournament CreateTournament(string file, string content)
		{
			var fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
			                                    	{ file, new MockFileData(content) }
			                                    });
				
			return new Tournament(fileSystem);
		}

		[Test]
		public void TestRead_ShouldReadFileIfExists()
		{
			string wc2014 = @"name = ""VM 2014 Brasil""";
			var t = CreateTournament(@"data\vm2014.toml", wc2014);
			Should.NotThrow(() => t.Read(@"data\vm2014.toml"));
		}
		
		[Test]
		public void TestRead_ShouldFailIfFileDoesNotExists()
		{
			string wc2014 = @"name = ""VM 2014 Brasil""";
			var t = CreateTournament(@"data\vm2014.toml", wc2014);
			Should.Throw<FileNotFoundException>(() => t.Read(@"configs\vm2014.toml"));
		}
		
		[Test]
		public void TestGetName_ShouldReturn_TournamentName()
		{
			string file = @"data\vm2014.toml";

			string wc2014 = @"name = ""VM 2014 Brasil""";
			var t = CreateTournament(file, wc2014);
			t.Read(file);
			string name = t.GetName();
			name.ShouldBe("VM 2014 Brasil");

			string notwc2014 = @"name = ""Another cup that is not VM 2014 Brasil""";
			t = CreateTournament(file, notwc2014);
			t.Read(file);
			name = t.GetName();
			name.ShouldBe("Another cup that is not VM 2014 Brasil");
		}
		
		[Test]
		public void TestGetGroups_ShouldReturnAllGroupsInTournament()
		{
			string wc2014 = @"name = ""VM 2014 Brasil""
groups = [
	[""Brasil"",		""Kroatia"", 		""Mexico"", 			""Kamerun""],
	[""Spania"",		""Nederland"", 	""Chile"", 			""Australia""],
	[""Colombia"",	""Hellas"", 		""Elfenbenskysten"", 	""Japan""],
	[""Uruguay"",		""Costa Rica"",	""England"", 			""Italia""],
	[""Sveits"",		""Ecuador"", 		""Frankrike"",		""Honduras""],
	[""Argentina"",	""Bosnia"", 		""Iran"", 			""Nigeria""],
	[""Tyskland"",	""Portugal"", 	""Ghana"", 			""USA""],
	[""Belgia"",		""Algerie"", 		""Russland"",			""Soer-Korea""],
]";
			var t = CreateTournament(@"data\vm2014.toml", wc2014);
			t.Read(@"data\vm2014.toml");
			var groups = t.GetGroups();
			groups.Length.ShouldBe(8);
		}
		
	}
}