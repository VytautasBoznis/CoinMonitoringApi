using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinMonitoringPortalApi.Data.Messages.Account
{
	public class RegisterRequest
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
	}

	public class RegisterResponse
	{
		public bool Success { get; set; }
		public string Error { get; set; }
	}
}
