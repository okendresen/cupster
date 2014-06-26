/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 20.06.2014
 * Time: 22:20
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
		
	}
}
