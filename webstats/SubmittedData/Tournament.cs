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
		public Tournament(IFileSystem fileSystem)
		{
			_fileSystem = fileSystem;
		}
		
		public Tournament() : this(
			fileSystem: new FileSystem()
		)
		{
		}

        dynamic _config;
        public Tournament(dynamic config)
            : this(
                fileSystem: new FileSystem()
            )
        {
            _config = config;
        }
			
		public void Load(string file)
		{
			string text = _fileSystem.File.ReadAllText(file);
			_config = text.ParseAsToml();
		}
		
		public string GetName()
		{
			return _config.name;
		}

        public TournamentType GetTheType()
        {
            if (_config.type == "uefa-euro")
            {
                return TournamentType.UEFA_Euro;
            }
            else
            {
                return TournamentType.FIFA_WordCup;
            }
        }
        
        public bool IsFifaWorldCup()
        {
            return GetTheType() == TournamentType.FIFA_WordCup;
        }
        
		public object[] GetGroups()
		{
			return _config.groups;
		}
	}
}