using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinMonitoringPortalApi.Data.Messages.Trade;

namespace CoinMonitoringPortalApi.Data.Messages.Elastic
{
	public class GetFormattedDataRequest
	{
		public string PatterName { get; set; }
		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public int ExchangeType { get; set; }
		public int PairType { get; set; }
		public int size { get; set; }
	}

	public class GetFormattedDataResponse
	{
		public int took { get; set; }
		public ElasticHitWrapper<TickerFormattedDto> hits { get; set; }
	}
}
