using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CoinMonitoringApi.Interfaces.Exchanges;
using CoinMonitoringPortalApi.Data.Messages.CoinManagers;
using RestSharp;

namespace CoinMonitoringPortalApi.Business.Exchanges
{
	public class CexManager: ICexManager
	{
		public readonly RestClient _client;
		public readonly string _cexApi = ConfigurationManager.AppSettings["CexApi"];

		public CexManager()
		{
			_client = new RestClient(_cexApi);
		}

		public CexTradeResponse PerformTrade(CexTradeRequest request)
		{
			RestRequest restRequest = new RestRequest("place_order/"+ request.Symbol1 +"/"+request.Symbol2, Method.POST, DataFormat.Json);

			string[] keySplit = request.Key.Split(',');
			string signature = CreateSignature(request.Nonce, keySplit[0], keySplit[1], request.Secret);

			request.Key = keySplit[1];
			restRequest.AddJsonBody(new Dictionary<string, string>{
				{"key", request.Key},
				{"signature", signature},
				{"nonce", request.Nonce},
				{"type", request.Type},
				{"amount", request.Amount.ToString()},
				{"price", request.Price.ToString()},
			});

			IRestResponse<CexTradeResponse> restResponse = _client.Execute<CexTradeResponse>(restRequest);

			return restResponse.Data ?? new CexTradeResponse
			{
				Completed = false
			};
		}

		public CexBalanceResponse GetBalance(CexBalanceRequest request)
		{
			string[] keySplit = request.Key.Split(',');
			string signature = CreateSignature(request.Nonce, keySplit[0], keySplit[1], request.Secret);

			RestRequest restRequest = new RestRequest("balance/", Method.POST, DataFormat.Json);
			request.Key = keySplit[1];
			restRequest.AddJsonBody(new Dictionary<string, string>{
				{"key", request.Key},
				{"signature", signature},
				{"nonce", request.Nonce}
			});

			IRestResponse<CexBalanceResponse> restResponse = _client.Execute<CexBalanceResponse>(restRequest);

			return restResponse.Data;
		}

		public string CreateSignature(string nonce, string username, string key, string secret)
		{
			byte[] bites = new HMACSHA256(Encoding.UTF8.GetBytes(secret)).ComputeHash(Encoding.UTF8.GetBytes(nonce + username + key));
			
			return BitConverter.ToString(bites).ToUpper().Replace("-", string.Empty); ;
		}
	}
}
