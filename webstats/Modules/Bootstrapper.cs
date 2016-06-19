/*
 * Created by SharpDevelop.
 * User: Lars Magnus
 * Date: 14.06.2014
 * Time: 23:23
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Diagnostics;
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
            string dataPath = ConfigurationManager.AppSettings["DataPath"].ToString();
            string tournamentFile = ConfigurationManager.AppSettings["TournamentFile"].ToString();
            string resultsFile = ConfigurationManager.AppSettings["ResultsFile"].ToString();
		    
            // Register our app dependency as a normal singletons
            var tournament = new Tournament();
            tournament.Load(Path.Combine(dataPath, tournamentFile));
            container.Register<ITournament, Tournament>(tournament);
			
            var bets = new SubmittedBets();
            bets.TournamentFile = tournamentFile;
            bets.ActualResultsFile = resultsFile;
            bets.LoadAll(dataPath);
            container.Register<ISubmittedBets, SubmittedBets>(bets);

            var results = new Results();
            results.Load(Path.Combine(dataPath, resultsFile));
            container.Register<IResults, Results>(results);
        }

		protected override DiagnosticsConfiguration DiagnosticsConfiguration
		{
			get { return new DiagnosticsConfiguration { Password = @"kokko-bada-futu" }; }
		}

		#if DEBUG
		protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
		{
			StaticConfiguration.DisableErrorTraces = false;
		}
		#endif
    }
}
