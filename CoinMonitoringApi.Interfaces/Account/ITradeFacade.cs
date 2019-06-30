using CoinMonitoringPortalApi.Data.Messages.Trade;

namespace CoinMonitoringApi.Interfaces.Account
{
	public interface ITradeFacade
	{
		GetScheduledTradesResponse GetScheduledTrades(GetScheduledTradesRequest request);
		CreateScheduledTradesResponse CreateScheduledTrade(CreateScheduledTradesRequest request);
		DeleteScheduledTradeResponse DeleteScheduledTrade(DeleteScheduledTradeRequest request);
		ResetScheduledTradeResponse ResetScheduledTrade(ResetScheduledTradeRequest request);
		SynchronizePortfolioResponse SynchronizePortfolio(SynchronizePortfolioRequest request);
		GetChartDataResponse GetChartData(GetChartDataRequest request);
		RecalculateActionsResponse RecalculateActions(RecalculateActionsRequest request);
	}
}
