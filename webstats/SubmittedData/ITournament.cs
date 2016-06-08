/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 14.06.2014
 * Time: 20:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace SubmittedData
{
	/// <summary>
	/// Description of ITournament.
	/// </summary>
	
	public enum TournamentType { FIFA_WordCup, UEFA_Euro }
	
	public interface ITournament
	{
		void Load(string file);
		string GetName();
		TournamentType GetTheType();
		bool IsFifaWorldCup();
		object[] GetGroups();		
	}
}
