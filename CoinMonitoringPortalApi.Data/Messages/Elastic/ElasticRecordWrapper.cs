using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinMonitoringPortalApi.Data.Messages.Elastic
{
	public class ElasticRecordWrapper<T>
	{
		public string _index { get; set; }
		public string _type { get; set; }
		public string _id { get; set; }
		public string _score { get; set; }
		public T _source { get; set; }
	}
}
