using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinMonitoringPortalApi.Data.Messages.Trade
{
	public class DeleteScheduledTradeRequest
	{
		public int UserNr { get; set; }
		public int TradeNr { get; set; }
	}

	public class DeleteScheduledTradeResponse
	{
		public bool Success { get; set; }
		public string Error { get; set; }
	}
}
