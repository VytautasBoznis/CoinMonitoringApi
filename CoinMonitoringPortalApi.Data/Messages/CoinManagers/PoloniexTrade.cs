using System.Collections.Generic;
using CoinMonitoringPortalApi.Data.Data.Enums;
using CoinMonitoringPortalApi.Data.Data.Exchange;

namespace CoinMonitoringPortalApi.Data.Messages.CoinManagers
{
	public class PoloniexTradeRequest
	{
		public string Command { get; set; }
		public string CurrencyPair { get; set; }
		public decimal Rate { get; set; }
		public decimal Amount { get; set; }
		public string Nonce { get; set; }
	}

	public class PoloniexTradeResponse
	{
		public long OrderNumber { get; set; }
		public List<PoloniexTradeData> ResultingTrades { get; set; }
		public decimal AmountUnfilled { get; set; }
		public decimal Fee { get; set; }
		public string CurrencyPair { get; set; }
	}
}
