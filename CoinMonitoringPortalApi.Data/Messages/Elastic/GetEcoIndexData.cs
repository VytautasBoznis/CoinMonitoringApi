using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinMonitoringPortalApi.Data.Messages.Trade;

namespace CoinMonitoringPortalApi.Data.Messages.Elastic
{
	public class GetEcoIndexDataRequest
	{
		public string PatterName { get; set; }
		public DateTime From { get; set; }
		public DateTime To { get; set; }
		public int EcoIndexNr { get; set; }
		public string PairName { get; set; }
		public int size { get; set; }
	}

	public class GetEcoIndexDataResponse
	{
		public int took { get; set; }
		public ElasticHitWrapper<EcoIndex> hits { get; set; }
	}
}
