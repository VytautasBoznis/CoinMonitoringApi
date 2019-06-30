using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinMonitoringPortalApi.Data.Messages.Elastic
{
	public class ElasticFormattedShouldFilter
	{
		public ElasticRange range { get; set; }
	}
}
