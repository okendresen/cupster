/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 12.06.2014
 * Time: 20:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using SubmittedData;
namespace Modules
{
	public class IndexPageViewModel
	{
		ITournament _tournament;
		
		public IndexPageViewModel(ITournament t, ISubmittedBets sb, IResults actual)
		{
			_tournament = t;
			CreateGroups();
			CreateBetterlist(sb.GetBetters(), sb, actual);
			MarkWinnerIfFinished(actual);
			TimeStamp = actual.GetTimeStamp();
		}

		public string PageTitle
		{
			get { return _tournament.GetName(); }
		}

		public object Tournament
		{
			get { return _tournament.GetName(); }
		}
		List<Group> _groups = new List<Group>();
		public List<Group> Groups
		{
			get { return _groups; }
			private set { _groups = value; }
		}

        List<Better> _betters = new List<Better>();
        public List<Better> Betters 
        {
            get { return _betters; }
            private set { _betters = value; }
        }

        public List<Better> SortedBetters
        {
            get { return _betters.OrderByDescending(b => b.Score).ToList(); }
        }
        
        public string TimeStamp { get; private set; }

		void CreateBetterlist(List<string> betters, ISubmittedBets sb, IResults actual)
		{
		    foreach (var better in betters)
		    {
		        var score = new ScoringSystem(sb.GetSingleBet(better), actual);
		        var bet = new Better() { Name = better, Score = score.GetTotal() };
		        var achievements = new AchievementSystem(sb.GetSingleBet(better), actual);
		        bet.Achievements = achievements.Achievements;
		        bet.RowClass = "normal";
		        Betters.Add(bet);
		    }
		}

		void MarkWinnerIfFinished(IResults actual)
		{
		    Better leader = _betters.OrderByDescending(b => b.Score).ToList()[0];
		    if (actual.HasFinal())
		        leader.RowClass = "success";
		}

		private void CreateGroups()
		{
			char gn = 'A';
			foreach (object[] group in _tournament.GetGroups())
			{
				var g = new Group() { Name = "Group " + gn };
				foreach (var team in group)
				{
					g.Teams.Add(team.ToString());
				}
				_groups.Add(g);
				gn++;
			}
		}
		
		public class Group
		{
			public string Name { get; set; }
			
			List<string> _teams = new List<string>();
			public List<string> Teams
			{
				get { return _teams; }
				private set { _teams = value; }
			}
			
			public string TeamsAsHtml
			{
				get {
					StringBuilder s = new StringBuilder();
					s.Append("<ul class=\"list-group\">");
					s.AppendLine();
					foreach (var team in Teams)
					{
						s.AppendFormat("		<li class=\"list-group-item\">{0}</li>", team);
						s.AppendLine();
					}
					s.Append("	</ul>");
					return s.ToString();
				}
			}
		}
		
		public class Better
		{
		    public string Name { get; set; }
		    public int Score { get; set; }
		    public string RowClass { get; set; }
		    public List<AchievementSystem.Achievement> Achievements;
		    public string AchievementsAsHtml 
		    {
		        get 
		        {
		            StringBuilder s = new StringBuilder();
		            foreach (var ach in Achievements) 
		            {
		                s.AppendFormat("<img src=\"Content/{0}\" title=\"{1}\">", ach.Image, ach.Title);
		            }
		            return s.ToString();
		        }
		    }
		}
	}
}


