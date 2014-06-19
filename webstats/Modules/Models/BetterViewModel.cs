using SubmittedData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modules
{
    public class BetterViewModel
    {
		dynamic _bet;

        public BetterViewModel(ITournament t, dynamic bet)
        {
        	_bet = bet;
        }

		public object Better
		{
			get { return _bet.info.user; }
		}

		public object PageTitle
		{
			get { return _bet.info.user; }
		}
    }
}
