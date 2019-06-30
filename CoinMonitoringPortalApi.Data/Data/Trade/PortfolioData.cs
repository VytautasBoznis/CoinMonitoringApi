using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinMonitoringPortalApi.Data.Data.Trade
{
	public class PortfolioData
	{
		public int ExchangeType { get; set; }
		public List<CurrencyData> CurrencyDatas { get; set; }
	}
}
