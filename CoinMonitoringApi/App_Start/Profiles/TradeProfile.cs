using AutoMapper;
using CoinMonitoringPortalApi.Business.Database.Context;
using CoinMonitoringPortalApi.Data.Data.Trade;

namespace CoinMonitoringApi.App_Start.Profiles
{
	public class TradeProfile : Profile
	{
		public TradeProfile()
		{
			CreateMap<Trade_Trades, ScheduledTrade>().ReverseMap();
			CreateMap<Exchange_Pairs, ExchangePair>().ReverseMap();
			CreateMap<Trade_Criteria, TradeCriteria>().ReverseMap();
		}
	}
}