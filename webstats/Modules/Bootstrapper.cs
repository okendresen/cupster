/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 14.06.2014
 * Time: 23:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Nancy;
using Nancy.TinyIoc;
using SubmittedData;

namespace Modules
{
	/// <summary>
	/// Description of Bootstrapper.
	/// </summary>
	public class Bootstrapper : DefaultNancyBootstrapper
	{
		protected override void ConfigureApplicationContainer(TinyIoCContainer container)
		{
			// Register our app dependency as a normal singletons
			var tournament = new Tournament();
			tournament.Load(@"../../../../data/vm2014.toml");
			container.Register<ITournament, Tournament>(tournament);
			
			var bets = new SubmittedBets();
			bets.TournamentFile = "vm2014.toml";
			bets.ActualResultsFile = "vm2014-actual.toml";
			bets.LoadAll(@"../../../../data");
			container.Register<ISubmittedBets, SubmittedBets>(bets);

			var results = new Results();
			results.Load(@"../../../../data\vm2014-actual.toml");
			container.Register<IResults, Results>(results);
		}
	}
}
