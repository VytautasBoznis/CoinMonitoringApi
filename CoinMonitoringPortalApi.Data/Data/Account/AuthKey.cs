namespace CoinMonitoringPortalApi.Data.Data.Account
{
	public class AuthKey
	{
		public int KeyNr { get; set; }

		public int UserNr { get; set; }

		public int ExchangeType { get; set; }
		
		public string KeyValue { get; set; }
		
		public string SecretValue { get; set; }
	}
}
