using System.Collections.Generic;
using System.Configuration;
using CoinMonitoringApi.Interfaces.Database;
using CoinMonitoringPortalApi.Data.Messages.Elastic;
using RestSharp;

namespace CoinMonitoringPortalApi.Business.Database.Elastic
{
	public class ElasticClient: IElasticClient
	{
		private readonly IRestClient _client;
		public readonly string _elasticApi = ConfigurationManager.AppSettings["ElasticApi"];

		public ElasticClient()
		{
			_client = new RestClient(_elasticApi);
		}

		public GetFormattedDataResponse GetFormattedTickers(GetFormattedDataRequest request)
		{
			RestRequest restRequest = new RestRequest(request.PatterName + "/_search", Method.POST);

			ElasticMultiRequest searchRequest = new ElasticMultiRequest
			{
				query = new ElasticFormattedQuery
				{
					@bool = new ElasticFormattedBool
					{
						filter = new ElasticFormattedFilter
						{
							term = new ElasticTermExchangeNr
							{
								ExchangeType = request.ExchangeType
							}
						},
						must = new ElasticFormattedMustFilter
						{
							term = new ElasticMustPair
							{
								PairType = request.PairType
							}
						},
						should = new ElasticFormattedShouldFilter
						{
							range = new ElasticRange
							{
								Time = new ElasticRangeParameter
								{
									gte = request.From.ToString("yyyy-MM-dd"),
									lte = request.To.ToString("yyyy-MM-dd")
								}
							}
						}
					}
				},
				size = request.size,
				sort = new List<ElasticSort>
				{
					new ElasticSort
					{
						Time = new ElasticTimeSort()
					}
				}
			};

			restRequest.AddJsonBody(searchRequest);
			IRestResponse<GetFormattedDataResponse> response = _client.Execute<GetFormattedDataResponse>(restRequest);
			return response.Data;
		}

		public GetEcoIndexDataResponse GetEcoIndexData(GetEcoIndexDataRequest request)
		{
			RestRequest restRequest = new RestRequest(request.PatterName + "/_search", Method.POST);

			ElasticSingleRequest searchRequest = new ElasticSingleRequest
			{
				query = new ElasticEcoQuery
				{
					@bool = new ElasticBool
					{
						filter = new ElasticFilter
						{
							term = new ElasticTermId
							{
								Id = request.EcoIndexNr
							}
						},
						must = new ElasticMustFilter
						{
							term = new ElasticMustName
							{
								Name = request.PairName
							}
						},
						should = new ElasticShouldFilter()
						{
							range = new ElasticRange
							{
								Time = new ElasticRangeParameter
								{
									gte = request.From.ToString("yyyy-MM-dd"),
									lte = request.To.ToString("yyyy-MM-dd")
								}
							}
						}
					}
				},
				size = request.size,
				sort = new List<ElasticSort>
				{
					new ElasticSort
					{
						Time = new ElasticTimeSort()
					}
				}
			};

			restRequest.AddJsonBody(searchRequest);
			IRestResponse<GetEcoIndexDataResponse> response = _client.Execute<GetEcoIndexDataResponse>(restRequest);
			return response.Data;
		}
	}
}
