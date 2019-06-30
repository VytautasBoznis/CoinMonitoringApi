using System.Collections.Generic;

namespace CoinMonitoringPortalApi.Data.Messages.Elastic
{
	public class ElasticSearchRequest
	{
		public ElasticQuery query { get; set; }
		public int size => 500;
		public List<ElasticSort> sort { get; set; }
	}
}
