using SubmittedData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modules
{
    public class BetterViewModel
    {
		dynamic _bet;
		ITournament _tournament;
		
        public BetterViewModel(ITournament t, dynamic bet)
        {
        	_tournament = t;
        	_bet = bet;
        	CreateMatches();
        }

		public object Better
		{
			get { return _bet.info.user; }
		}

		public object PageTitle
		{
			get { return _bet.info.user; }
		}

		List<Group> _groups = new List<Group>();
		public List<Group> Groups
		{
			get { return _groups; }
			private set { _groups = value; }
		}

		private void CreateMatches()
		{
			char gn = 'A';
			foreach (object[] group in _tournament.GetGroups())
			{
				var g = new Group() { Name = "Group " + gn };
				g.Matches.Add(new Tuple<string, string>(group[0].ToString(), group[1].ToString()));
				g.Matches.Add(new Tuple<string, string>(group[2].ToString(), group[3].ToString()));
				g.Matches.Add(new Tuple<string, string>(group[0].ToString(), group[2].ToString()));
				g.Matches.Add(new Tuple<string, string>(group[3].ToString(), group[1].ToString()));
				g.Matches.Add(new Tuple<string, string>(group[3].ToString(), group[0].ToString()));
				g.Matches.Add(new Tuple<string, string>(group[1].ToString(), group[2].ToString()));
				_groups.Add(g);
				gn++;
			}
		}
		
		
		public class Group
		{
			public string Name { get; set; }
			
			List<Tuple<string, string>> _matches = new List<Tuple<string, string>>();
			public List<Tuple<string, string>> Matches
			{
				get { return _matches; }
				private set { _matches = value; }
			}
			
			public string MatchesAsHtml
			{
				get {
					StringBuilder s = new StringBuilder();
					s.Append("<table>");
					s.AppendLine();
					s.Append("<tr>\n<th>Match</th>\n<th>Selected</th><th>Result</th>");
					foreach (var match in Matches)
					{
						s.Append("<tr>");
						s.AppendLine();
						s.AppendFormat("	<td>{0} vs. {1}</td>", match.Item1, match.Item2);
						s.AppendLine();
						s.AppendFormat("	<td></td>");
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
		}
    }
}
