namespace CoinMonitoringPortalApi.Data.Data.Trade
{
	public class TradeCriteria
	{
		public int CriteriaNr { get; set; }
		public int TradeNr { get; set; }
		public int EcoIndexType { get; set; }
		public int CriteriaValueType { get; set; }
		public decimal Value { get; set; }
		public decimal Weight { get; set; }
	}
}
