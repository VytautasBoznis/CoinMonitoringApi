using System.Collections.Generic;

namespace CoinMonitoringPortalApi.Data.Messages.CoinManagers
{
	public class PoloniexBalanceRequest
	{
		public string Command => "returnBalances";
		public string Nonce { get; set; }
	}

	public class PoloniexBalanceResponse
	{
		public Dictionary<string, decimal> Balances { get; set; }
	}
}
