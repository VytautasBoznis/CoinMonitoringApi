using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinMonitoringPortalApi.Data.Data.Trade
{
	public class ExchangePair
	{
		public int PairNr { get; set; }
		public int ExchangeType { get; set; }
		public string Symbol1 { get; set; }
		public string Symbol2 { get; set; }
	}
}
