/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 14.06.2014
 * Time: 20:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace SubmittedData
{
	/// <summary>
	/// Description of ISubmittedBets.
	/// </summary>
	public interface ISubmittedBets
	{
		int Count { get; }
		string TournamentFile { get; set; }
		bool LoadAll(string folder);
		List<string> GetBetters();
		dynamic GetSingleBet(string user);
	}
}
