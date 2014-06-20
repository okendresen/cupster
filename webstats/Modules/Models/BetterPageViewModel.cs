using SubmittedData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modules
{
	public class BetterPageViewModel
	{
		dynamic _bet;
		ITournament _tournament;
		IResults _results;
		
		public BetterPageViewModel(ITournament t, dynamic bet, IResults actual)
		{
			_tournament = t;
			_bet = bet;
			_results = actual;
			CreateGroupMatches();
		}

		public object Better
		{
			get { return _bet.info.user; }
		}

		public object PageTitle
		{
			get { return _bet.info.user; }
		}

		List<GroupMatches> _groups = new List<GroupMatches>();
		public List<GroupMatches> Groups
		{
			get { return _groups; }
			private set { _groups = value; }
		}

		private void CreateGroupMatches()
		{
			char gn = 'A';
			int i = 0;
			foreach (object[] group in _tournament.GetGroups())
			{
				dynamic stageOne = ((IDictionary<String, Object>)_bet)["stage-one"];
				dynamic stageOneActual = _results.GetStageOne();
				var g = new GroupMatches() { Name = "Group " + gn };
				g.CreateMatches(group, stageOne.results[i], stageOneActual.results[i]);
				_groups.Add(g);
				gn++;
				i++;
			}
		}
		
		
		public class GroupMatches
		{
			public string Name { get; set; }
			
			List<Tuple<string, string, string, string>> _matches = new List<Tuple<string, string, string, string>>();
			
			public string MatchesAsHtml
			{
				get {
					StringBuilder s = new StringBuilder();
					s.Append("<table>");
					s.AppendLine();
					s.Append("<tr>\n<th>Match</th>\n<th>Selected</th><th>Result</th>");
					foreach (var match in _matches)
					{
						s.Append("<tr>");
						s.AppendLine();
						s.AppendFormat("	<td>{0} vs. {1}</td>", match.Item1, match.Item2);
						s.AppendLine();
						s.AppendFormat("	<td>{0}</td>", GetResults(match));
						s.AppendLine();
						s.AppendFormat("	<td>{0}</td>", GetResults(match, match.Item4));
						s.AppendLine();
						s.Append("</tr>");
						s.AppendLine();
					}
					s.Append("	</table>");
					return s.ToString();
				}
			}
			
			public void CreateMatches(object[] group, object[] results, object[] actuals)
			{
				_matches.Add(new Tuple<string, string, string, string>(group[0].ToString(), group[1].ToString(), 
				                                                       results[0].ToString(), actuals[0].ToString()));
				_matches.Add(new Tuple<string, string, string, string>(group[2].ToString(), group[3].ToString(), 
				                                                       results[1].ToString(), actuals[1].ToString()));
				_matches.Add(new Tuple<string, string, string, string>(group[0].ToString(), group[2].ToString(), 
				                                                       results[2].ToString(), actuals[2].ToString()));
				_matches.Add(new Tuple<string, string, string, string>(group[3].ToString(), group[1].ToString(), 
				                                                       results[3].ToString(), actuals[3].ToString()));
				_matches.Add(new Tuple<string, string, string, string>(group[3].ToString(), group[0].ToString(), 
				                                                       results[4].ToString(), actuals[4].ToString()));
				_matches.Add(new Tuple<string, string, string, string>(group[1].ToString(), group[2].ToString(), 
				                                                       results[5].ToString(), actuals[5].ToString()));
			}

			public string GetResults(Tuple<string, string, string, string> match)
			{
				return GetResults(match, match.Item3);
			}
			public string GetResults(Tuple<string, string, string, string> match, string result)
			{
				if (result.ToLower().Equals("h"))
					return match.Item1;
				else if (result.ToLower().Equals("b"))
					return match.Item2;
				else if (result.ToLower().Equals("u"))
					return "Draw";
				else
					return "";
			}
		}

	}
}
