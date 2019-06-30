using CoinMonitoringPortalApi.Data.Data.Exchange;

namespace CoinMonitoringPortalApi.Data.Messages.CoinManagers
{
	public class CexBalanceRequest
	{
		public string Key { get; set; }
		public string Secret { get; set; }
		public string Nonce { get; set; }
	}

	public class CexBalanceResponse
	{
		public string Timestamp { get; set; }
		public string Username { get; set; }
		public CexPortfolioData BTC { get; set; }
		public CexPortfolioData ETH { get; set; }
		public CexPortfolioData EUR { get; set; }
		public CexPortfolioData USD { get; set; }
	}
}
