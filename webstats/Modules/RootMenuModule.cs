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
		private ITournament _tournament;
		public RootMenuModule(ITournament tournament)
		{
			_tournament = tournament;

			Get["/"] = _ => PrintGroups();
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
