using CoinMonitoringPortalApi.Data.Data.Account;

namespace CoinMonitoringPortalApi.Data.Messages.Account
{
	public class CreateAuthKeyRequest
	{
		public AuthKey AuthKey { get; set; }
	}

	public class CreateAuthKeyResponse
	{
		public bool Success { get; set; }
		public string Error { get; set; }
	}
}
