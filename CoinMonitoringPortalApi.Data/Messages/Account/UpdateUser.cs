using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinMonitoringPortalApi.Data.Messages.Account
{
	public class UpdateUserRequest
	{
		public int UserNr { get; set; }
		public string Email { get; set; }
	}

	public class UpdateUserResponse
	{
		public bool Success { get; set; }
		public string Error { get; set; }
	}
}
