using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinMonitoringPortalApi.Data.Messages.Trade
{
	public class RecalculateActionsRequest
	{
	}

	public class RecalculateActionsResponse
	{
		public bool Success { get; set; }
		public string Error { get; set; }
	}
}
