namespace CoinMonitoringPortalApi.Data.Messages.Elastic
{
	public class ElasticBool
	{
		public ElasticFilter filter { get; set; }
		public ElasticMustFilter must { get; set; }
		public ElasticShouldFilter should { get; set; }
	}
}
