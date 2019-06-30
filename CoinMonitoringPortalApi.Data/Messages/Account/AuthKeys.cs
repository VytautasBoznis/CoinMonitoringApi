using System.Collections.Generic;
using CoinMonitoringPortalApi.Data.Data.Account;

namespace CoinMonitoringPortalApi.Data.Messages.Account
{
	public class AuthKeysRequest
	{
		public int UserNr { get; set; }
	}

	public class AuthKeysResponse
	{
		public bool Success { get; set; }
		public string Error { get; set; }
		public List<AuthKey> AuthKeys { get; set; }
	}
}
