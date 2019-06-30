using System.Collections.Generic;
using CoinMonitoringPortalApi.Data.Data.Trade;

namespace CoinMonitoringPortalApi.Data.Messages.Trade
{
	public class GetScheduledTradesRequest
	{
		public int UserNr { get; set; }
	}

	public class GetScheduledTradesResponse
	{
		public bool Success { get; set; }
		public string Error { get; set; }
		public List<ScheduledTrade> Trades { get; set; }
	}
}
