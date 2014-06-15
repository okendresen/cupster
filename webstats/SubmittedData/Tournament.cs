/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 12.06.2014
 * Time: 23:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO.Abstractions;
using System.Collections.Generic;
using Toml;

namespace SubmittedData
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class Tournament : ITournament
	{
		readonly IFileSystem _fileSystem;
		dynamic _config;
		
		public Tournament(IFileSystem fileSystem)
		{
			_fileSystem = fileSystem;
		}
		
		public Tournament() : this(
			fileSystem: new FileSystem()
		)
		{
		}
			
		public void Read(string file)
		{
			string text = _fileSystem.File.ReadAllText(file);
			_config = text.ParseAsToml();
		}
		
		public string GetName()
		{
			return _config.name;
		}

		public object[] GetGroups()
		{
			return _config.groups;
		}
	}
}