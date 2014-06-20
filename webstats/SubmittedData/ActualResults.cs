/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 20.06.2014
 * Time: 22:25
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using Toml;

namespace SubmittedData
{
	/// <summary>
	/// Description of ActualResults.
	/// </summary>
	public class ActualResults : IResults
	{
		readonly IFileSystem _fileSystem;
		public ActualResults(IFileSystem fileSystem)
		{
			_fileSystem = fileSystem;
		}
		public ActualResults() : this(
			fileSystem: new FileSystem()
		)
		{
		}

		dynamic _config;

		#region IResults implementation
		public void Load(string file)
		{
			string text = _fileSystem.File.ReadAllText(file);
			_config = text.ParseAsToml();
		}

		public dynamic GetStageOne()
		{
			return ((IDictionary<String, Object>)_config)["stage-one"];
		}
		#endregion

	}
}
