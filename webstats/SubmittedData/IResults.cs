﻿/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 14.06.2014
 * Time: 20:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace SubmittedData
{
	/// <summary>
	/// Description of IResults.
	/// </summary>
	public interface IResults
	{
		void Load(string file);

		dynamic GetStageOne();
		dynamic GetInfo();
		bool HasRound16();
		dynamic GetRound16Winners();
		bool HasQuarterFinals();
		dynamic GetQuarterFinalWinners();
		bool HasSemiFinals();
		dynamic GetSemiFinalWinners();
		List<string> GetBronseFinalists();
		bool HasBronseFinal();
		string GetBronseFinalWinner();
    }
}
