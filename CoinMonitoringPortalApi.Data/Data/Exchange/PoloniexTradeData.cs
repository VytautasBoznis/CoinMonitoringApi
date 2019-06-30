using System;

namespace CoinMonitoringPortalApi.Data.Data.Exchange
{
	public class PoloniexTradeData
	{
		public decimal Amount { get; set; }
		public DateTime Date { get; set; }
		public decimal Rate { get; set; }
		public decimal Total { get; set; }
		public long TradeID { get; set; }
		public string Type { get; set; }
	}
}
