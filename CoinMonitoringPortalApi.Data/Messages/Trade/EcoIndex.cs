using System;
using CoinMonitoringPortalApi.Data.Data.Enums;

namespace CoinMonitoringPortalApi.Data.Messages.Trade
{
	public class EcoIndex
	{
		public EcoIndexEnum Id { get; set; }
		public string Name { get; set; }
		public DateTime Time { get; set; }
		public decimal Value { get; set; }
	}
}
