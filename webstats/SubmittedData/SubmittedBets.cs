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
	public class SubmittedBets : ISubmittedBets
	{
		public string TournamentFile { get; set; }

		readonly IFileSystem _fileSystem;
		List<string> _fileNames;
        Dictionary<string, dynamic> _submitted = new Dictionary<string, dynamic>();

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
					if (TournamentFile == null || (!file.Contains(TournamentFile)))
					{
						var text = _fileSystem.File.ReadAllText(file);
                        var bet = text.ParseAsToml();
                        _submitted[bet.info.user] = bet;
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
			foreach (var better in _submitted.Keys)
			{
				betters.Add(better);
			}
			
			return betters;
		}

		public dynamic GetSingleBet(string user)
		{
			return _submitted[user];
		}
	}
}
