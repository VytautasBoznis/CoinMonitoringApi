using System.Collections.Generic;

namespace CoinMonitoringPortalApi.Data.Messages.Elastic
{
	public class ElasticHitWrapper<T>
	{
		public long total { get; set; }
		public List<ElasticRecordWrapper<T>> hits { get; set; }
	}
}
