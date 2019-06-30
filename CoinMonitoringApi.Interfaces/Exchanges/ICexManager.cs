using CoinMonitoringPortalApi.Data.Messages.CoinManagers;

namespace CoinMonitoringApi.Interfaces.Exchanges
{
	public interface ICexManager
	{
		CexTradeResponse PerformTrade(CexTradeRequest request);
		CexBalanceResponse GetBalance(CexBalanceRequest request);
	}
}
