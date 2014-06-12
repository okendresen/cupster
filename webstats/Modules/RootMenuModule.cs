/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 12.06.2014
 * Time: 20:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Nancy;

namespace webstats
{
	/// <summary>
	/// Description of WebService.
	/// </summary>
	public class WebService : NancyModule
	{
		public WebService()
		{
			Get["/"] = _ => "Welcome screen and menu selection.";
		}
	}
}
