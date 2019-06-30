using System.Collections.Generic;
using CoinMonitoringPortalApi.Data.Data.Account;
using CoinMonitoringPortalApi.Data.Data.Trade;

namespace CoinMonitoringPortalApi.Data.Messages.Account
{
	public class LoginRequest
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}

	public class LoginResponse
	{
		public bool Success { get; set; }
		public string Error { get; set; }
		public User User { get; set; }
		public List<PortfolioData> PortfolioDatas { get; set; }
	}
}
