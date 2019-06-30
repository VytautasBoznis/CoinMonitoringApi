using System.Collections.Generic;
using CoinMonitoringPortalApi.Data.Data.Trade;

namespace CoinMonitoringPortalApi.Data.Messages.Trade
{
	public class SynchronizePortfolioRequest
	{
		public int UserNr { get; set; }
	}

	public class SynchronizePortfolioResponse
	{
		public bool Success { get; set; }
		public string Error { get; set; }
		public List<PortfolioData> PortfolioData { get; set; }
	}
}
