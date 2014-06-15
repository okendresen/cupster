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
using System.Text;
using Nancy;
using SubmittedData;
namespace Modules
{
	public class GroupsViewModel
	{
		ITournament _tournament;
		
		public string Tournament
		{
			get { return _tournament.GetName(); }
		}

		List<Group> _groups = new List<Group>();
		public List<Group> Groups
		{
			get { return _groups; }
			private set { _groups = value; }
		}
		
		public GroupsViewModel(ITournament t)
		{
			_tournament = t;
			CreateGroups();
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
					s.Append("<ul>");
					s.AppendLine();
					foreach (var team in Teams)
					{
						s.AppendFormat("		<li>{0}</li>", team);
						s.AppendLine();
					}
					s.Append("	</ul>");
					return s.ToString();
				}
			}
		}
	}
}


