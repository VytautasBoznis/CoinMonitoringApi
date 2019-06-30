using System;
using CoinMonitoringPortalApi.Data.Data.Enums;

namespace CoinMonitoringPortalApi.Data.Messages.Trade
{
	public class TickerFormattedDto
	{
		public ExchangeTypeEnum ExchangeType { get; set; }
		public ExchangePairTypeEnum PairType { get; set; }
		public DateTime Time { get; set; }
		public decimal Low { get; set; }
		public decimal High { get; set; }
		public decimal Last { get; set; }
		public decimal Volume { get; set; }
	}
}
