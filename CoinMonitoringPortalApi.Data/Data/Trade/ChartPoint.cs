using System;

namespace CoinMonitoringPortalApi.Data.Data.Trade
{
	public class ChartPoint
	{
		public DateTime Time { get; set; }
		public decimal High { get; set; }
		public decimal Low { get; set; }
		public decimal Last { get; set; }
		public decimal Volume { get; set; }
	}
}
