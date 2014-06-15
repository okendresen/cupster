/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 12.06.2014
 * Time: 20:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;
using Nancy;
using SubmittedData;

namespace Modules
{
	/// <summary>
	/// Description of WebService.
	/// </summary>
	public class RootMenuModule : NancyModule
	{
		public RootMenuModule(ITournament tournament)
		{
			Get["/"] = _ => {
				return View["groups.sshtml", new GroupsViewModel(tournament)];
			};
		}
	}
	
	public class GroupsViewModel
	{
		public string Tournament 
		{
			get { return _tournament.GetName(); }
		}

		ITournament _tournament;
		public GroupsViewModel(ITournament t)
		{
			_tournament = t;
		}

		private string PrintGroups()
		{
			StringBuilder s = new StringBuilder();
			s.AppendFormat("Welcome to {0} betting scores\n", _tournament.GetName());
			
			char gn = 'A';
			foreach (object[] group in _tournament.GetGroups())
			{
				s.AppendLine("Group " + gn);
				foreach (var team in group) 
				{
					s.AppendLine(team.ToString());
				}
				gn++;
			}
			
			return s.ToString();
		}
	}
}
