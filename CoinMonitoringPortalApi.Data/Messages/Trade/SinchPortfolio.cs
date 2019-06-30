using System.Collections.Generic;
using CoinMonitoringPortalApi.Data.Data.Trade;

namespace CoinMonitoringPortalApi.Data.Messages.Trade
{
	public class SynchPortfolioRequest
	{
	}

	public class SynchPortfolioResponse
	{
		public bool Success { get; set; }
		public string Error { get; set; }
		private List<PortfolioData> PortfolioDatas { get; set; }
	}
}
