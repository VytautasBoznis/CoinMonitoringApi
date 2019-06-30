using CoinMonitoringPortalApi.Data.Messages.CoinManagers;

namespace CoinMonitoringApi.Interfaces.Exchanges
{
	public interface IPoloniexManager
	{
		PoloniexTradeResponse PerformTrade(PoloniexTradeRequest request, string key, string secret);
		PoloniexBalanceResponse GetBalances(PoloniexBalanceRequest request, string key, string secret);
	}
}
