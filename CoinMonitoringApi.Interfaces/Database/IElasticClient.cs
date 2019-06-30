using CoinMonitoringPortalApi.Data.Messages.Elastic;

namespace CoinMonitoringApi.Interfaces.Database
{
	public interface IElasticClient
	{
		GetFormattedDataResponse GetFormattedTickers(GetFormattedDataRequest request);
		GetEcoIndexDataResponse GetEcoIndexData(GetEcoIndexDataRequest request);
	}
}
