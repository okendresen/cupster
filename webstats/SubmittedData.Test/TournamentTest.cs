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
		[Test]
		public void TestRead_ShouldReadFile()
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
			var fileSystem = new MockFileSystem( new Dictionary<string, MockFileData>
			                                    {
			                                    	{ @"data\vm2014.toml", new MockFileData(wc2014) }
			                                    });
				
			var t = new Tournament(fileSystem);
			string content = t.Read(@"data\vm2014.toml");
			content.ShouldBe(wc2014);
		}
	}
}