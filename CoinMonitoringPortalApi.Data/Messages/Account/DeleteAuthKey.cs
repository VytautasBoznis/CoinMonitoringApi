using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinMonitoringPortalApi.Data.Messages.Account
{
	public class DeleteAuthKeyRequest
	{
		public int UserNr { get; set; }
		public int KeyNr { get; set; }
	}

	public class DeleteAuthKeyResponse
	{
		public bool Success { get; set; }
		public string Error { get; set; }
	}
}
