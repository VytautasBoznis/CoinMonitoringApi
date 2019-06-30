using System.Web.Http;
using CoinMonitoringApi.Interfaces.Account;
using CoinMonitoringPortalApi.Data.Messages.Account;
using CoinMonitoringPortalApi.Data.Messages.Trade;

namespace CoinMonitoringApi.Controllers
{
    public class TradeController : ApiController
    {
	    private ITradeFacade _tradeFacade;

	    public TradeController(ITradeFacade tradeFacade)
	    {
		    _tradeFacade = tradeFacade;
	    }

	    [Route("Trade/GetScheduledTrades")]
	    [HttpPost]
	    public GetScheduledTradesResponse GetScheduledTrades([FromBody] GetScheduledTradesRequest request)
	    {
		    GetScheduledTradesResponse response = _tradeFacade.GetScheduledTrades(request);
		    return response;
	    }

		[Route("Trade/CreateScheduledTrade")]
	    [HttpPost]
	    public CreateScheduledTradesResponse CreateScheduledTrade([FromBody] CreateScheduledTradesRequest request)
	    {
		    CreateScheduledTradesResponse response = _tradeFacade.CreateScheduledTrade(request);
		    return response;
	    }

	    [Route("Trade/DeleteScheduledTrade")]
	    [HttpPost]
	    public DeleteScheduledTradeResponse DeleteScheduledTrade([FromBody] DeleteScheduledTradeRequest request)
	    {
		    DeleteScheduledTradeResponse response = _tradeFacade.DeleteScheduledTrade(request);
		    return response;
	    }

	    [Route("Trade/ResetScheduledTrade")]
	    [HttpPost]
	    public ResetScheduledTradeResponse ResetScheduledTrade([FromBody] ResetScheduledTradeRequest request)
	    {
		    ResetScheduledTradeResponse response = _tradeFacade.ResetScheduledTrade(request);
		    return response;
	    }

	    [Route("Trade/SynchronizePortfolio")]
	    [HttpPost]
	    public SynchronizePortfolioResponse SynchronizePortfolio([FromBody] SynchronizePortfolioRequest request)
	    {
		    SynchronizePortfolioResponse response = _tradeFacade.SynchronizePortfolio(request);
		    return response;
	    }

	    [Route("Trade/GetChartData")]
	    [HttpPost]
	    public GetChartDataResponse GetChartData([FromBody] GetChartDataRequest request)
	    {
		    GetChartDataResponse response = _tradeFacade.GetChartData(request);
		    return response;
	    }

	    [Route("Trade/RecalculateActions")]
	    [HttpPost]
	    public RecalculateActionsResponse RecalculateActions([FromBody] RecalculateActionsRequest request)
	    {
		    RecalculateActionsResponse response = _tradeFacade.RecalculateActions(request);
		    return response;
	    }
	}
}
