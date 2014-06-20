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
		
		public BetterPageViewModel(ITournament t, dynamic bet)
		{
			_tournament = t;
			_bet = bet;
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
				var g = new GroupMatches() { Name = "Group " + gn };
				g.CreateMatches(group, stageOne.results[i]);
				_groups.Add(g);
				gn++;
				i++;
			}
		}
		
		
		public class GroupMatches
		{
			public string Name { get; set; }
			
			List<Tuple<string, string, string>> _matches = new List<Tuple<string, string, string>>();
			
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
						s.AppendFormat("	<td></td>");
						s.AppendLine();
						s.Append("</tr>");
						s.AppendLine();
					}
					s.Append("	</table>");
					return s.ToString();
				}
			}
			
			public void CreateMatches(object[] group, object[] results)
			{
				_matches.Add(new Tuple<string, string, string>(group[0].ToString(), group[1].ToString(), results[0].ToString()));
				_matches.Add(new Tuple<string, string, string>(group[2].ToString(), group[3].ToString(), results[1].ToString()));
				_matches.Add(new Tuple<string, string, string>(group[0].ToString(), group[2].ToString(), results[2].ToString()));
				_matches.Add(new Tuple<string, string, string>(group[3].ToString(), group[1].ToString(), results[3].ToString()));
				_matches.Add(new Tuple<string, string, string>(group[3].ToString(), group[0].ToString(), results[4].ToString()));
				_matches.Add(new Tuple<string, string, string>(group[1].ToString(), group[2].ToString(), results[5].ToString()));
			}

			public string GetResults(Tuple<string, string, string> match)
			{
				if (match.Item3.Equals("h"))
					return match.Item1;
				else if (match.Item3.Equals("b"))
					return match.Item2;
				else
					return "Draw";
			}
		}

	}
}
