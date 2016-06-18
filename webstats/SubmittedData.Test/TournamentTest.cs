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
		public void TestLoad_ShouldReadFile_IfExists()
		{
			string wc2014 = @"name = ""VM 2014 Brasil""";
			var t = CreateTournament(@"data\vm2014.toml", wc2014);
			Should.NotThrow(() => t.Load(@"data\vm2014.toml"));
		}
		
		[Test]
		public void TestLoad_ShouldFail_IfFileDoesNotExists()
		{
			string wc2014 = @"name = ""VM 2014 Brasil""";
			var t = CreateTournament(@"data\vm2014.toml", wc2014);
			Should.Throw<FileNotFoundException>(() => t.Load(@"configs\vm2014.toml"));
		}
		
		[Test]
		public void TestGetName_ShouldReturnTournamentName()
		{
			string file = @"data\vm2014.toml";

			string wc2014 = @"name = ""VM 2014 Brasil""";
			var t = CreateTournament(file, wc2014);
			t.Load(file);
			string name = t.GetName();
			name.ShouldBe("VM 2014 Brasil");

			string notwc2014 = @"name = ""Another cup that is not VM 2014 Brasil""";
			t = CreateTournament(file, notwc2014);
			t.Load(file);
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
			t.Load(@"data\vm2014.toml");
			var groups = t.GetGroups();
			groups.Length.ShouldBe(8);

			string smallerwc2014 = @"name = ""VM 2014 Brasil""
groups = [
	[""Brasil"",		""Kroatia"", 		""Mexico"", 			""Kamerun""],
	[""Spania"",		""Nederland"", 	""Chile"", 			""Australia""],
]";
			t = CreateTournament(@"data\vm2014.toml", smallerwc2014);
			t.Load(@"data\vm2014.toml");
			groups = t.GetGroups();
			groups.Length.ShouldBe(2);
		}
		
	   [Test]
	   public void TestGetTheType_WhenGiven_ShouldReturnCorrectTournamentType()
	   {
            string file = @"data\euro2016.toml";
            string cup = @"name = ""Euro 2016 France""
type = ""uefa-euro""";
            var t = CreateTournament(file, cup);
            t.Load(file);
            TournamentType type = t.GetTheType();
            type.ShouldBe(TournamentType.UEFA_Euro);

            cup = @"name = ""World Cup 2016""
type = ""fifa-wouldcup""";
            t = CreateTournament(file, cup);
            t.Load(file);
            type = t.GetTheType();
            type.ShouldBe(TournamentType.FIFA_WordCup);
	   }

       [Test]
       public void TestGetTheType_Default_ShouldReturnFifa()
       {
            string file = @"data\euro2016.toml";
            string cup = @"name = ""World Cup 2016""
type = ""fifa-wouldcup""";
            var t = CreateTournament(file, cup);
            t.Load(file);
            TournamentType type = t.GetTheType();
            type.ShouldBe(TournamentType.FIFA_WordCup);
       }
       
       [Test]
       public void TestIsFifaWorldCup_ShouldReturnFalse_WhenTypeIsUefa()
       {
            string file = @"data\euro2016.toml";
            string cup = @"name = ""Euro 2016 France""
type = ""uefa-euro""";
            var t = CreateTournament(file, cup);
            t.Load(file);
            t.IsFifaWorldCup().ShouldBe(false);
       }

       [Test]
       public void TestIsUefaEuro_ShouldReturnTrue_WhenTypeIsFifa()
       {
            string file = @"data\euro2016.toml";
            string cup = @"name = ""World Cup 2016""
type = ""fifa-wouldcup""";
            var t = CreateTournament(file, cup);
            t.Load(file);
            t.IsFifaWorldCup().ShouldBe(true);
       }

		[Test]
		public void TestFindGroup_ShouldReturnGroupOfTeam()
		{
			string file = @"data\euro2016.toml";
			string toml = @"name = ""foo""
groups = [ [""op"", ""oop""], [""opo"", ""ioi""] ]";
			var t = CreateTournament(file, toml);
			t.Load(file);

			t.FindGroup("oop").ShouldBe('A');
			t.FindGroup("opo").ShouldBe('B');
			t.FindGroup("op").ShouldBe('A');
			t.FindGroup("not_there").ShouldBe('\0');
		}
	}
}