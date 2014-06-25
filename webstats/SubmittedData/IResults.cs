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
	/// Description of IResults.
	/// </summary>
	public interface IResults
	{
		void Load(string file);

		dynamic GetStageOne();
		dynamic GetInfo();
		dynamic GetRound16();
		bool HasRound16();
	}
}
