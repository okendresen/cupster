/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 14.06.2014
 * Time: 15:42
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
	/// Description of SubmittedBets.
	/// </summary>
	public class SubmittedBets
	{
		public string Tournament { get; set; }

		readonly IFileSystem _fileSystem;
		List<string> _fileNames;
		List<dynamic> _submitted = new List<dynamic>();

		public SubmittedBets(IFileSystem fileSystem)
		{
			_fileSystem = fileSystem;
		}

		public SubmittedBets() : this (
			fileSystem: new FileSystem()
		)
		{
		}

		public bool LoadAll(string folder)
		{
			if (_fileSystem.Directory.Exists(folder))
			{
				_fileNames = new List<string>(_fileSystem.Directory.GetFiles(folder));
				foreach(var file in _fileNames)
				{
					if (Tournament == null || (!file.Contains(Tournament)))
					{
						var text = _fileSystem.File.ReadAllText(file);
						_submitted.Add(text.ParseAsToml());
					}
				}
				return true;
			}
			else
			{
				return false;
			}
		}

		public int Count
		{
			get { return _submitted.Count; }
		}

		public List<string> GetBetters()
		{
			List<string> betters = new List<string>();
			foreach (var bet in _submitted) 
			{
				betters.Add(bet.info.user);
			}
			
			return betters;
		}
	}
}
