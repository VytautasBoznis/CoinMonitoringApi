namespace CoinMonitoringPortalApi.Data.Messages.Elastic
{
	public class ElasticFormattedBool
	{
		public ElasticFormattedFilter filter { get; set; }
		public ElasticFormattedMustFilter must { get; set; }
		public ElasticFormattedShouldFilter should { get; set; }
	}
}
