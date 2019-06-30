using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinMonitoringPortalApi.Data.Messages.Elastic
{
	public class ElasticMultiRequest
	{
		public ElasticFormattedQuery query { get; set; }
		public int size { get; set; }
		public List<ElasticSort> sort { get; set; }
	}
}
