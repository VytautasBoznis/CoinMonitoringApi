namespace CoinMonitoringPortalApi.Data.Messages.CoinManagers
{
	public class CexTradeRequest
	{
		public string Symbol1 { get; set; }
		public string Symbol2 { get; set; }
		public string Nonce { get; set; }
		public string Type { get; set; }
		public decimal Amount { get; set; }
		public decimal Price { get; set; }
		public string Key { get; set; }
		public string Secret { get; set; }
	}

	public class CexTradeResponse
	{
		public bool Completed { get; set; }
	}
}
