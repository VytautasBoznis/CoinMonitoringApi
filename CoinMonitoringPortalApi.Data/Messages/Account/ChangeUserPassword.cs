namespace CoinMonitoringPortalApi.Data.Messages.Account
{
	public class ChangeUserPasswordRequest
	{
		public int UserNr { get; set; }
		public string NewPassword { get; set; }
	}

	public class ChangeUserPasswordResponse
	{
		public bool Success { get; set; }
		public string Error { get; set; }
	}
}
