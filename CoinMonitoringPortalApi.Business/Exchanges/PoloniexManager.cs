using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CoinMonitoringApi.Interfaces.Exchanges;
using CoinMonitoringPortalApi.Data.Data.Exchange;
using CoinMonitoringPortalApi.Data.Messages.CoinManagers;
using RestSharp;

namespace CoinMonitoringPortalApi.Business.Exchanges
{
	public class PoloniexManager: IPoloniexManager
	{
		public readonly RestClient _client;
		public readonly string _poloniexUrl = ConfigurationManager.AppSettings["PoloniexApi"];

		public PoloniexManager()
		{
			_client = new RestClient(_poloniexUrl);
		}
		
		public PoloniexTradeResponse PerformTrade(PoloniexTradeRequest request, string key, string secret)
		{
			string command = "command=" + request.Command + "&currencyPair="+ request.CurrencyPair + "&rate=" + request.Rate + "&amount=" + request.Amount.ToString("00.00000") + "&nonce=" + request.Nonce;
			string signature = CreateSignature(secret, command);

			RestRequest restRequest = new RestRequest("tradingApi", Method.POST);
			restRequest.AddHeader("Key", key);
			restRequest.AddHeader("Sign", signature);
			restRequest.AddParameter("command", request.Command);
			restRequest.AddParameter("currencyPair", request.CurrencyPair);
			restRequest.AddParameter("rate", request.Rate);
			restRequest.AddParameter("amount", request.Amount.ToString("00.00000"));
			restRequest.AddParameter("nonce", request.Nonce);

			IRestResponse<PoloniexTradeResponse> restResponse = _client.Execute<PoloniexTradeResponse>(restRequest);

			return restResponse.Data ?? new PoloniexTradeResponse{
				ResultingTrades = new List<PoloniexTradeData>()
			};
		}

		public PoloniexBalanceResponse GetBalances(PoloniexBalanceRequest request, string key, string secret)
		{
			string command = "command=" + request.Command + "&nonce=" + request.Nonce;
			string signature = CreateSignature(secret, command);

			RestRequest restRequest = new RestRequest("tradingApi", Method.POST);
			restRequest.AddHeader("Key", key);
			restRequest.AddHeader("Sign", signature);
			restRequest.AddParameter("command", request.Command);
			restRequest.AddParameter("nonce", request.Nonce);

			IRestResponse<Dictionary<string, decimal>> restResponse = _client.Execute<Dictionary<string, decimal>>(restRequest);

			return  new PoloniexBalanceResponse
			{
				Balances = restResponse.Data
			};
		}

		private string CreateSignature(string secret, string command)
		{
			byte[] bites = new HMACSHA512(Encoding.UTF8.GetBytes(secret)).ComputeHash(Encoding.UTF8.GetBytes(command));

			return string.Concat(bites.Select(b => b.ToString("X2")));
		}
	}
}
