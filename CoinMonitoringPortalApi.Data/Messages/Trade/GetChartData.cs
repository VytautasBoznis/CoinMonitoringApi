using System;
using System.Collections.Generic;
using CoinMonitoringPortalApi.Data.Data.Trade;

namespace CoinMonitoringPortalApi.Data.Messages.Trade
{
	public class GetChartDataRequest
	{
		public int ExchangeType { get; set; }
		public string Symbol1 { get; set; }
		public string Symbol2 { get; set; }
		public DateTime From { get; set; }
		public DateTime? To { get; set; }
		public bool ShowRSI { get; set; }
		public bool ShowEMA { get; set; }
		public bool ShowFI { get; set; }
	}

	public class GetChartDataResponse
	{
		public bool Success { get; set; }
		public string Error { get; set; }
		public List<ChartPoint> ChartPoints { get; set; }
		public Dictionary<int, List<EcoPoint>> EcoPoints { get; set; }
	}
}
