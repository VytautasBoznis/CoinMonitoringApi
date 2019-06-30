using System.Collections.Generic;

namespace CoinMonitoringPortalApi.Data.Messages.Elastic
{
	public class ElasticSingleRequest
	{
		public ElasticEcoQuery query { get; set; }
		public int size { get; set; }
		public List<ElasticSort> sort { get; set; }
	}
}
