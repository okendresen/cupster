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
using System.Globalization;
using System.IO.Abstractions;
using System.Linq;
using Toml;

namespace SubmittedData
{
    /// <summary>
    /// Description of ActualResults.
    /// </summary>

    public class Results : IResults
    {
        readonly IFileSystem _fileSystem;
        public Results(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }
        public Results()
            : this(
                fileSystem: new FileSystem()
            )
        {
        }


        dynamic _results;
        public Results(dynamic results)
            : this(
                fileSystem: new FileSystem()
            )
        {
            _results = results;
        }


        #region IResults implementation

        public void Load(string file)
        {
            string text = _fileSystem.File.ReadAllText(file);
            _results = text.ParseAsToml();
            _timeStamp = _fileSystem.File.GetLastWriteTime(file).ToString("F", CultureInfo.InvariantCulture);
        }

        string _timeStamp;
		public string GetTimeStamp()
		{
		    return _timeStamp;
		}
		
		public bool HasStageOne()
		{
			return ((IDictionary<String, Object>)_results).ContainsKey("stage-one");
		}
        public dynamic GetStageOne()
        {
            return ((IDictionary<String, Object>)_results)["stage-one"];
        }

        public dynamic GetInfo()
        {
            return _results.info;
        }

		public bool HasStageTwo()
		{
			return ((IDictionary<String, Object>)_results).ContainsKey("stage-two");
		}
		
        public bool HasRound16()
        {
            if (((IDictionary<String, Object>)_results).ContainsKey("stage-two"))
            {
                var st = ((IDictionary<String, Object>)_results)["stage-two"]; 
                return ((IDictionary<String, Object>)st).ContainsKey("round-of-16");
		        
            } else
            {
                return false;
            }
        }
        public dynamic GetRound16Winners()
        {
            var st = ((IDictionary<String, Object>)_results)["stage-two"]; 
            return ((IDictionary<String, Object>)st)["round-of-16"];
        }

        public bool HasQuarterFinals()
        {
            if (((IDictionary<String, Object>)_results).ContainsKey("stage-two"))
            {
                var st = ((IDictionary<String, Object>)_results)["stage-two"]; 
                return ((IDictionary<String, Object>)st).ContainsKey("quarter-final");
		        
            } else
            {
                return false;
            }
        }

        public dynamic GetQuarterFinalWinners()
        {
            var st = ((IDictionary<String, Object>)_results)["stage-two"]; 
            return ((IDictionary<String, Object>)st)["quarter-final"];
        }


        public bool HasSemiFinals()
        {
            if (((IDictionary<String, Object>)_results).ContainsKey("stage-two"))
            {
                var st = ((IDictionary<String, Object>)_results)["stage-two"]; 
                return ((IDictionary<String, Object>)st).ContainsKey("semi-final");
		        
            } else
            {
                return false;
            }
        }
        public dynamic GetSemiFinalWinners()
        {
            var st = ((IDictionary<String, Object>)_results)["stage-two"]; 
            return ((IDictionary<String, Object>)st)["semi-final"];
        }
    	
        public List<string> GetBronseFinalists()
        {
            List<string> bfinalists = new List<string>();
            foreach (var team in GetQuarterFinalWinners())
            {
                object[] sf = GetSemiFinalWinners();
                if (Array.IndexOf(sf, team) == -1)
                {
                    bfinalists.Add(team);
                }
            }
            return bfinalists;
        }

        public bool HasBronseFinal()
        {
            if (((IDictionary<String, Object>)_results).ContainsKey("finals"))
            {
                return ((IDictionary<String, Object>)_results.finals).ContainsKey("bronse-final");
		        
            } else
            {
                return false;
            }
        }

        public string GetBronseFinalWinner()
        {
            return ((IDictionary<String, Object>)_results.finals)["bronse-final"].ToString();
        }
        
        public bool HasFinal()
        {
            if (((IDictionary<String, Object>)_results).ContainsKey("finals"))
            {
                return ((IDictionary<String, Object>)_results.finals).ContainsKey("final");
		        
            } else
            {
                return false;
            }
        }
        
        public string GetFinalWinner()
        {
            return _results.finals.final.ToString();
        }

        #endregion
    }
}
