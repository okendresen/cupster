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

namespace SubmittedData
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class Tournament
	{
		string _file;
		readonly IFileSystem _fileSystem;

		public Tournament(IFileSystem fileSystem)
		{
			_fileSystem = fileSystem;
		}
		
		public Tournament() : this(
			fileSystem: new FileSystem()
		)
		{
		}
			
		public string Read(string file)
		{
			string text = _fileSystem.File.ReadAllText(file);
			_file = file;
			return text;
		}
		
	}
}