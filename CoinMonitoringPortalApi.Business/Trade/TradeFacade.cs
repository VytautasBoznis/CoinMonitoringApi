using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoinMonitoringApi.Interfaces.Account;
using CoinMonitoringApi.Interfaces.Database;
using CoinMonitoringApi.Interfaces.Exchanges;
using CoinMonitoringPortalApi.Business.Database.Context;
using CoinMonitoringPortalApi.Business.Database.Elastic;
using CoinMonitoringPortalApi.Business.Database.Enums;
using CoinMonitoringPortalApi.Business.Exchanges;
using CoinMonitoringPortalApi.Data.Data.Enums;
using CoinMonitoringPortalApi.Data.Data.Trade;
using CoinMonitoringPortalApi.Data.Messages.CoinManagers;
using CoinMonitoringPortalApi.Data.Messages.Elastic;
using CoinMonitoringPortalApi.Data.Messages.Trade;
using log4net;
using CriteriaValueType = CoinMonitoringPortalApi.Business.Database.Enums.CriteriaValueType;
using CurrencyType = CoinMonitoringPortalApi.Business.Database.Enums.CurrencyType;
using EcoIndexType = CoinMonitoringPortalApi.Business.Database.Context.EcoIndexType;
using ExchangeTypeEnum = CoinMonitoringPortalApi.Business.Database.Enums.ExchangeTypeEnum;
using TradeActionType = CoinMonitoringPortalApi.Business.Database.Enums.TradeActionType;

namespace CoinMonitoringPortalApi.Business.Trade
{
	public class TradeFacade: ITradeFacade
	{
		private DatabaseContext db;
		private readonly IPoloniexManager _poloniexManager;
		private readonly ICexManager _cexManager;
		private readonly IElasticClient _elasticClient;
		private static ILog _logger;
		protected ILog Logger => _logger;

		public TradeFacade()
		{
			db = new DatabaseContext();
			_logger = LogManager.GetLogger(GetType().Name);
			_poloniexManager = new PoloniexManager();
			_cexManager = new CexManager();
			_elasticClient = new ElasticClient();
		}

		public GetScheduledTradesResponse GetScheduledTrades(GetScheduledTradesRequest request)
		{
			GetScheduledTradesResponse response = new GetScheduledTradesResponse
			{
				Success = true,
				Error = ""
			};

			try
			{
				List<Trade_Trades> trades = db.Trade_Trades.Where(t => t.UserNr == request.UserNr).ToList();

				if (trades.Count > 0)
				{
					List<ScheduledTrade> finalTrades = Mapper.Map<List<ScheduledTrade>>(trades);

					foreach (ScheduledTrade trade in finalTrades)
					{
						List<Trade_Criteria> criterias = db.Trade_Criteria.Where(c => c.TradeNr == trade.TradeNr).ToList();
						trade.TradeCriteria = Mapper.Map<List<TradeCriteria>>(criterias);

						Exchange_Pairs dbPair = db.Exchange_Pairs.FirstOrDefault(p => p.PairNr == trade.ExchangePairNr);
						if (dbPair == null)
						{
							response.Success = false;
							response.Error = $"Wrong format trade request found {trade.TradeNr} remove it manually before all trades can be loaded";
							return response;
						}

						trade.ExchangePair = Mapper.Map<ExchangePair>(dbPair);
					}

					response.Trades = finalTrades;
				}
			}
			catch (Exception e)
			{
				_logger.ErrorFormat($"error happend error: {e.Message} stacktrace: {e.StackTrace}");
				response.Success = false;
				response.Error = "An error occured in user creation please check logs for more details";
			}

			return response;
		}

		public CreateScheduledTradesResponse CreateScheduledTrade(CreateScheduledTradesRequest request)
		{
			CreateScheduledTradesResponse response = new CreateScheduledTradesResponse
			{
				Success = true,
				Error = ""
			};

			if (request.Trade == null)
			{
				response.Success = false;
				response.Error = "No trade provided";
				return response;
			}

			List<Trade_Criteria> newCriterias = new List<Trade_Criteria>();

			foreach (var tradeCriteria in request.Trade.TradeCriteria)
			{
				newCriterias.Add(new Trade_Criteria
				{
					EcoIndexType = tradeCriteria.EcoIndexType,
					CriteriaValueType = tradeCriteria.CriteriaValueType,
					Value = tradeCriteria.Value,
					Weight = tradeCriteria.Weight
				});
			}
			
			Exchange_Pairs pair = db.Exchange_Pairs.FirstOrDefault(p => p.Symbol1.ToLower() == request.Trade.ExchangePair.Symbol1.ToLower() && 
			                                                            p.Symbol2.ToLower() == request.Trade.ExchangePair.Symbol2.ToLower() && 
			                                                            p.ExchangeType == request.Trade.ExchangePair.ExchangeType);

			Trade_Trades newTrade = new Trade_Trades
			{
				UserNr = request.UserNr,
				CreationDate = DateTime.Now,
				Value = request.Trade.Value,
				ExchangePairNr = pair.PairNr,
				TradeAction = request.Trade.TradeAction,
				TradeStatus = (int) TradeStatusTypeEnum.Pending,
				Trade_Criteria = newCriterias
			};

			try
			{
				db.Trade_Trades.Add(newTrade);
				db.SaveChanges();
			}
			catch (Exception e)
			{
				_logger.ErrorFormat($"error happend error: {e.Message} stacktrace: {e.StackTrace}");
				response.Success = false;
				response.Error = "An error occured in user creation please check logs for more details";
			}

			return response;
		}

		public DeleteScheduledTradeResponse DeleteScheduledTrade(DeleteScheduledTradeRequest request)
		{
			DeleteScheduledTradeResponse response = new DeleteScheduledTradeResponse
			{
				Success = true,
				Error = ""
			};

			Trade_Trades trade = db.Trade_Trades.FirstOrDefault(t => t.TradeNr == request.TradeNr && t.UserNr == request.UserNr);

			if (trade == null)
			{
				response.Success = false;
				response.Error = "No such trade found";
				return response;
			}

			List<Trade_Criteria> criterias = db.Trade_Criteria.Where(c => c.TradeNr == trade.TradeNr).ToList();

			try
			{
				db.Trade_Criteria.RemoveRange(criterias);
				db.Trade_Trades.Remove(trade);
				db.SaveChanges();
			}
			catch (Exception e)
			{
				_logger.ErrorFormat($"error happend error: {e.Message} stacktrace: {e.StackTrace}");
				response.Success = false;
				response.Error = "An error occured in user creation please check logs for more details";
			}

			return response;
		}

		public ResetScheduledTradeResponse ResetScheduledTrade(ResetScheduledTradeRequest request)
		{
			ResetScheduledTradeResponse response = new ResetScheduledTradeResponse
			{
				Success = true,
				Error = ""
			};

			Trade_Trades trade = db.Trade_Trades.FirstOrDefault(t => t.TradeNr == request.TradeNr && t.UserNr == request.UserNr);

			if (trade == null)
			{
				response.Success = false;
				response.Error = "No such trade found";
				return response;
			}

			try
			{
				trade.TradeStatus = (int) TradeStatusTypeEnum.Pending;
				db.SaveChanges();
			}
			catch (Exception e)
			{
				_logger.ErrorFormat($"error happend error: {e.Message} stacktrace: {e.StackTrace}");
				response.Success = false;
				response.Error = "An error occured in user creation please check logs for more details";
			}

			return response;
		}

		public SynchronizePortfolioResponse SynchronizePortfolio(SynchronizePortfolioRequest request)
		{
			SynchronizePortfolioResponse response = new SynchronizePortfolioResponse
			{
				Success = true,
				Error = "",
				PortfolioData = new List<PortfolioData>()
			};

			List<User_Keys> userKeys = db.User_Keys.Where(k => k.UserNr == request.UserNr).ToList();

			if (userKeys.Count == 0)
			{
				response.Success = false;
				response.Error = "User does not have any authorization keys";
				return response;
			}

			foreach (User_Keys key in userKeys)
			{
				if (key.ExchangeType == (int) ExchangeTypeEnum.Cex)
				{
					CexBalanceRequest cexBalanceRequest = new CexBalanceRequest
					{
						Key = key.KeyValue,
						Nonce = (154264078495300 + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()).ToString(),
						Secret = key.SecretValue
					};

					CexBalanceResponse cexBalanceResponse = _cexManager.GetBalance(cexBalanceRequest);
					List<User_Balances> balances = db.User_Balances.Where(b => b.UserNr == request.UserNr && b.ExchangeType == (int) ExchangeTypeEnum.Cex).ToList();
					
					if (balances.Count > 0)
					{
						db.User_Balances.RemoveRange(balances);
						db.SaveChanges();
					}

					if (cexBalanceResponse.BTC != null)
					{
						User_Balances btcBalances = new User_Balances
						{
							UserNr = request.UserNr,
							CurrencyType = (int) CurrencyType.BTC,
							ExchangeType = (int) ExchangeTypeEnum.Cex,
							Value = cexBalanceResponse.BTC.Available
						};

						db.User_Balances.Add(btcBalances);
					}

					if (cexBalanceResponse.ETH != null)
					{
						User_Balances ethBalances = new User_Balances
						{
							UserNr = request.UserNr,
							CurrencyType = (int)CurrencyType.ETH,
							ExchangeType = (int)ExchangeTypeEnum.Cex,
							Value = cexBalanceResponse.ETH.Available
						};

						db.User_Balances.Add(ethBalances);
					}

					if (cexBalanceResponse.EUR != null)
					{
						User_Balances eurBalances = new User_Balances
						{
							UserNr = request.UserNr,
							CurrencyType = (int)CurrencyType.EUR,
							ExchangeType = (int)ExchangeTypeEnum.Cex,
							Value = cexBalanceResponse.EUR.Available
						};

						db.User_Balances.Add(eurBalances);
					}

					if (cexBalanceResponse.USD != null)
					{
						User_Balances usdBalances = new User_Balances
						{
							UserNr = request.UserNr,
							CurrencyType = (int)CurrencyType.USD,
							ExchangeType = (int)ExchangeTypeEnum.Cex,
							Value = cexBalanceResponse.USD.Available
						};

						db.User_Balances.Add(usdBalances);
					}

					db.SaveChanges();
				}
				else
				{
					PoloniexBalanceRequest poloniexBalanceRequest = new PoloniexBalanceRequest
					{
						Nonce = (154264078495300 + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()).ToString()
					};

					PoloniexBalanceResponse poloniexBalanceResponse = _poloniexManager.GetBalances(poloniexBalanceRequest, key.KeyValue, key.SecretValue);

					List<User_Balances> balances = db.User_Balances.Where(b => b.UserNr == request.UserNr && b.ExchangeType == (int)ExchangeTypeEnum.Poloniex).ToList();

					if (balances.Count > 0)
					{
						db.User_Balances.RemoveRange(balances);
						db.SaveChanges();
					}

					foreach (var balance in poloniexBalanceResponse.Balances)
					{
						if (balance.Key == "BTC")
						{
							db.User_Balances.Add(new User_Balances
							{
								UserNr = request.UserNr,
								CurrencyType = (int) CurrencyType.BTC,
								ExchangeType = (int) ExchangeTypeEnum.Poloniex,
								Value = balance.Value
							});
						}
						if (balance.Key == "ETH")
						{
							db.User_Balances.Add(new User_Balances
							{
								UserNr = request.UserNr,
								CurrencyType = (int)CurrencyType.ETH,
								ExchangeType = (int)ExchangeTypeEnum.Poloniex,
								Value = balance.Value
							});
						}
						if (balance.Key == "USD")
						{
							db.User_Balances.Add(new User_Balances
							{
								UserNr = request.UserNr,
								CurrencyType = (int)CurrencyType.USD,
								ExchangeType = (int)ExchangeTypeEnum.Poloniex,
								Value = balance.Value
							});
						}
						if (balance.Key == "EUR")
						{
							db.User_Balances.Add(new User_Balances
							{
								UserNr = request.UserNr,
								CurrencyType = (int)CurrencyType.EUR,
								ExchangeType = (int)ExchangeTypeEnum.Poloniex,
								Value = balance.Value
							});
						}
					}
					
					db.SaveChanges();
				}
			}

			#region CEX Balances
			List<User_Balances> allBalances = db.User_Balances.Where(b => b.ExchangeType == (int)ExchangeTypeEnum.Cex && b.UserNr == request.UserNr).ToList();
			List<PortfolioData> portfolioDatas = new List<PortfolioData>
			{
				new PortfolioData
				{
					ExchangeType = (int) ExchangeTypeEnum.Cex,
					CurrencyDatas = new List<CurrencyData>()
				}
			};

			foreach (var balance in allBalances)
			{
				PortfolioData portfolioData = portfolioDatas.FirstOrDefault(p => p.ExchangeType == (int)ExchangeTypeEnum.Cex);
				portfolioData.CurrencyDatas.Add(new CurrencyData
				{
					Symbol = CurrencyTypeEnumToString(balance.CurrencyType),
					Value = balance.Value
				});
			}
			#endregion

			#region Poloniex Balances

			allBalances = db.User_Balances.Where(b => b.ExchangeType == (int)ExchangeTypeEnum.Poloniex && b.UserNr == request.UserNr).ToList();

			portfolioDatas.Add(new PortfolioData
			{
				ExchangeType = (int)ExchangeTypeEnum.Poloniex,
				CurrencyDatas = new List<CurrencyData>()
			});

			foreach (var balance in allBalances)
			{
				PortfolioData portfolioData = portfolioDatas.FirstOrDefault(p => p.ExchangeType == (int)ExchangeTypeEnum.Poloniex);
				portfolioData.CurrencyDatas.Add(new CurrencyData
				{
					Symbol = CurrencyTypeEnumToString(balance.CurrencyType),
					Value = balance.Value
				});
			}

			#endregion


			response.PortfolioData = portfolioDatas;

			return response;
		}

		public GetChartDataResponse GetChartData(GetChartDataRequest request)
		{
			GetFormattedDataRequest formattedDataRequest = new GetFormattedDataRequest
			{
				PatterName = request.ExchangeType == 1 ? "cex_formatted" : "poloniex_formatted",
				From = DateTime.Now.AddMinutes(-25),
				To = DateTime.Now,
				ExchangeType = request.ExchangeType,
				size = 300,
				PairType = (int) ResolvePairTypeEnum(request.Symbol1, request.Symbol2)
			};

			GetFormattedDataResponse formattedDataResponse = _elasticClient.GetFormattedTickers(formattedDataRequest);
			List<TickerFormattedDto> tickers = formattedDataResponse.hits.hits.Select(r => r._source).ToList();
			
			GetChartDataResponse response = new GetChartDataResponse
			{
				Success = true,
				Error = "",
				ChartPoints = new List<ChartPoint>(),
				EcoPoints = new Dictionary<int, List<EcoPoint>>
				{
					{ 1, new List<EcoPoint>() },
					{ 2, new List<EcoPoint>() },
					{ 3, new List<EcoPoint>() }
				}
			};

			foreach (TickerFormattedDto corTicker in tickers)
			{
				response.ChartPoints.Add(new ChartPoint
				{
					High = corTicker.High,
					Last = corTicker.Last,
					Low = corTicker.Low,
					Time = corTicker.Time,
					Volume = corTicker.Volume
				});
			}
			string pairName = request.Symbol1.ToLower() + "_" + request.Symbol2.ToLower();

			if (pairName == "btc_eth")
			{
				pairName = "eth_btc";
			}

			if (request.ShowRSI)
			{
				
				GetEcoIndexDataRequest ecoIndexDataRequest = new GetEcoIndexDataRequest
				{
					PatterName = request.ExchangeType == 1 ? "cex_eco_index" : "poloniex_eco_index",
					From = DateTime.Now.AddMinutes(-25),
					To = DateTime.Now,
					EcoIndexNr = (int)EcoIndexEnum.RSI,
					size = 300,
					PairName = pairName
				};

				GetEcoIndexDataResponse ecoIndexDataResponse = _elasticClient.GetEcoIndexData(ecoIndexDataRequest);
				List<EcoIndex> ecoIndexDtos = ecoIndexDataResponse.hits.hits.Select(r => r._source).ToList();

				List<EcoPoint> rsiPoints = response.EcoPoints.FirstOrDefault(k => k.Key == (int) EcoIndexEnum.RSI).Value;
				
				foreach (var ecoIndex in ecoIndexDtos)
				{
					rsiPoints.Add(new EcoPoint
					{
						Id = (int) EcoIndexEnum.RSI,
						Time = ecoIndex.Time,
						Value = ecoIndex.Value
					});
				}
			}

			if (request.ShowEMA)
			{
				GetEcoIndexDataRequest ecoIndexDataRequest = new GetEcoIndexDataRequest
				{
					PatterName = request.ExchangeType == 1 ? "cex_eco_index" : "poloniex_eco_index",
					From = DateTime.Now.AddMinutes(-25),
					To = DateTime.Now,
					EcoIndexNr = (int)EcoIndexEnum.EMA,
					size = 300,
					PairName = pairName
				};

				GetEcoIndexDataResponse ecoIndexDataResponse = _elasticClient.GetEcoIndexData(ecoIndexDataRequest);
				List<EcoIndex> ecoIndexDtos = ecoIndexDataResponse.hits.hits.Select(r => r._source).ToList();

				List<EcoPoint> emaPoints = response.EcoPoints.FirstOrDefault(k => k.Key == (int)EcoIndexEnum.EMA).Value;

				foreach (var ecoIndex in ecoIndexDtos)
				{
					emaPoints.Add(new EcoPoint
					{
						Id = (int)EcoIndexEnum.EMA,
						Time = ecoIndex.Time,
						Value = ecoIndex.Value
					});
				}
			}

			if (request.ShowFI)
			{
				GetEcoIndexDataRequest ecoIndexDataRequest = new GetEcoIndexDataRequest
				{
					PatterName = request.ExchangeType == 1 ? "cex_eco_index" : "poloniex_eco_index",
					From = DateTime.Now.AddMinutes(-25),
					To = DateTime.Now,
					EcoIndexNr = (int)EcoIndexEnum.ForceIndex,
					size = 300,
					PairName = pairName
				};

				GetEcoIndexDataResponse ecoIndexDataResponse = _elasticClient.GetEcoIndexData(ecoIndexDataRequest);
				List<EcoIndex> ecoIndexDtos = ecoIndexDataResponse.hits.hits.Select(r => r._source).ToList();

				List<EcoPoint> emaPoints = response.EcoPoints.FirstOrDefault(k => k.Key == (int)EcoIndexEnum.ForceIndex).Value;

				foreach (var ecoIndex in ecoIndexDtos)
				{
					emaPoints.Add(new EcoPoint
					{
						Id = (int)EcoIndexEnum.ForceIndex,
						Time = ecoIndex.Time,
						Value = ecoIndex.Value
					});
				}
			}

			return response;
		}
		
		public RecalculateActionsResponse RecalculateActions(RecalculateActionsRequest request)
		{
			List<Trade_Trades> allTrades = db.Trade_Trades.Where(t => t.TradeStatus != (int) TradeStatusTypeEnum.Failed && t.TradeStatus != (int)TradeStatusTypeEnum.Completed).ToList();
			Dictionary<int, List<EcoIndex>> allNewEcoIndexes = GetAllExchangeEcoIndexValues();
			
			foreach (Trade_Trades trade in allTrades)
			{
				List<Trade_Criteria> tradeCriterias = db.Trade_Criteria.Where(t => t.TradeNr == trade.TradeNr).ToList();
				Exchange_Pairs exchangePair = db.Exchange_Pairs.FirstOrDefault(t => t.PairNr == trade.ExchangePairNr);

				decimal successWeight = 0M;
				decimal totalWeight = tradeCriterias.Sum(tc => tc.Weight);
				if (exchangePair != null)
				{
					foreach (Trade_Criteria criteria in tradeCriterias)
					{
						EcoIndex criteriaIndex = allNewEcoIndexes.FirstOrDefault(i => i.Key == exchangePair.ExchangeType).Value.FirstOrDefault(e => (int) e.Id == criteria.EcoIndexType);

						if (criteriaIndex != null)
						{
							if (criteria.CriteriaValueType == (int)CriteriaValueType.Above)
							{
								if (criteriaIndex.Value > criteria.Value)
								{
									successWeight += criteria.Weight;
								}
							}
							else if (criteria.CriteriaValueType == (int)CriteriaValueType.AboveOrEqual)
							{
								if (criteriaIndex.Value >= criteria.Value)
								{
									successWeight += criteria.Weight;
								}
							}
							else if (criteria.CriteriaValueType == (int)CriteriaValueType.Equal)
							{
								if (criteriaIndex.Value == criteria.Value)
								{
									successWeight += criteria.Weight;
								}
							}
							else if (criteria.CriteriaValueType == (int)CriteriaValueType.BelowOrEqual)
							{
								if (criteriaIndex.Value <= criteria.Value)
								{
									successWeight += criteria.Weight;
								}
							}
							else if (criteria.CriteriaValueType == (int)CriteriaValueType.Below)
							{
								if (criteriaIndex.Value < criteria.Value)
								{
									successWeight += criteria.Weight;
								}
							}
						}

						if (successWeight >= totalWeight)
						{
							trade.TradeStatus = (int) TradeStatusTypeEnum.Valid;
							db.SaveChanges();

							if (exchangePair.ExchangeType == (int) ExchangeTypeEnum.Cex)
							{
								User_Keys key = db.User_Keys.FirstOrDefault(k => k.UserNr == trade.UserNr && k.ExchangeType == exchangePair.ExchangeType);

								if (key != null)
								{
									CexTradeRequest cexRequest = new CexTradeRequest
									{
										Amount = trade.Value,
										Nonce = (154264078495300 + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()).ToString(),
										Price = 1M,
										Symbol1 = exchangePair.Symbol1.ToUpper(),
										Symbol2 = exchangePair.Symbol2.ToUpper(),
										Type = trade.TradeAction == (int)TradeActionType.Buy ? "buy" : "sell",
										Key = key.KeyValue,
										Secret = key.SecretValue,
									};

									CexTradeResponse cexResponse = _cexManager.PerformTrade(cexRequest);

									if (cexResponse.Completed)
									{
										trade.TradeStatus = (int)TradeStatusTypeEnum.Completed;
										db.SaveChanges();
									}
									else
									{
										trade.TradeStatus = (int)TradeStatusTypeEnum.Failed;
										db.SaveChanges();
									}
								}
								else
								{
									trade.TradeStatus = (int)TradeStatusTypeEnum.Failed;
									db.SaveChanges();
								}
							}
							else
							{
								User_Keys key = db.User_Keys.FirstOrDefault(k => k.UserNr == trade.UserNr && k.ExchangeType == exchangePair.ExchangeType);

								if (key != null)
								{
									PoloniexTradeRequest poloniexRequest = new PoloniexTradeRequest
									{
										Amount = trade.Value,
										Command = trade.TradeAction == (int)TradeActionType.Buy ? "buy" : "sell",
										CurrencyPair = exchangePair.Symbol1.ToUpper() + "_" + exchangePair.Symbol2.ToUpper(),
										Nonce = (154264078495300 + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()).ToString(),
										Rate = 1M
									};

									PoloniexTradeResponse poloniexTradeResponse = _poloniexManager.PerformTrade(poloniexRequest, key.KeyValue, key.SecretValue);

									if (poloniexTradeResponse.ResultingTrades?.Count > 0)
									{
										trade.TradeStatus = (int)TradeStatusTypeEnum.Completed;
										db.SaveChanges();
									}
									else
									{
										trade.TradeStatus = (int)TradeStatusTypeEnum.Failed;
										db.SaveChanges();
									}
								}
								else
								{
									trade.TradeStatus = (int)TradeStatusTypeEnum.Failed;
									db.SaveChanges();
								}

							}
						}
					}
				}
			}

			return new RecalculateActionsResponse
			{
				Success = true,
				Error = ""
			};
		}

		private Dictionary<int, List<EcoIndex>> GetAllExchangeEcoIndexValues()
		{
			Dictionary<int, List<EcoIndex>> allIndexes = new Dictionary<int, List<EcoIndex>>
			{
				{1,new List<EcoIndex>()},
				{2,new List<EcoIndex>()},
			};

			List<string> allCurrencies = new List<string>
			{
				"btc_usd","eth_usd","eth_btc","eth_eur","btc_eur"
			};

			List<int> allEcoIndexes = new List<int>
			{
				(int)EcoIndexEnum.RSI,(int)EcoIndexEnum.EMA,(int)EcoIndexEnum.ForceIndex
			};

			foreach (var currIndex in allIndexes)
			{
				foreach (string currencyPair in allCurrencies)
				{
					foreach (int ecoIndex in allEcoIndexes)
					{
						GetEcoIndexDataRequest ecoIndexDataRequest = new GetEcoIndexDataRequest
						{
							PatterName = currIndex.Key == 1 ? "cex_eco_index" : "poloniex_eco_index",
							From = DateTime.Now.AddMinutes(-2),
							To = DateTime.Now,
							EcoIndexNr = ecoIndex,
							size = 1,
							PairName = currencyPair
						};

						GetEcoIndexDataResponse ecoIndexDataResponse = _elasticClient.GetEcoIndexData(ecoIndexDataRequest);
						EcoIndex newEcoIndex = ecoIndexDataResponse.hits.hits.FirstOrDefault()?._source;

						if (newEcoIndex != null)
						{
							currIndex.Value.Add(newEcoIndex);
						}
					}
				}
			}
			
			return allIndexes;
		}

		private string CurrencyTypeEnumToString(int currency)
		{
			switch (currency)
			{
				case 1: return "USD";
				case 2: return "EUR";
				case 3: return "BTC";
				case 4: return "ETH";
				default:
					return "UNKNOWN";
			}
		}

		private string ResolvePairTypeName(ExchangePairTypeEnum pairTypeEnum)
		{
			switch (pairTypeEnum)
			{
				case ExchangePairTypeEnum.BTC_USD:
					return "btc_usd";
				case ExchangePairTypeEnum.ETH_USD:
					return "eth_usd";
				case ExchangePairTypeEnum.ETH_BTC:
					return "eth_btc";
				case ExchangePairTypeEnum.ETH_EUR:
					return "eth_eur";
				case ExchangePairTypeEnum.BTC_EUR:
					return "btc_eur";
			}

			return "unknown";
		}

		private ExchangePairTypeEnum ResolvePairTypeEnum(string symbol1, string symbol2)
		{
			string name = symbol1.ToLower() + "_" + symbol2.ToLower();

			if (name == "btc_usd")
			{
				return ExchangePairTypeEnum.BTC_USD;
			}
			if (name == "eth_usd")
			{
				return ExchangePairTypeEnum.ETH_USD;
			}
			if (name == "eth_btc")
			{
				return ExchangePairTypeEnum.ETH_BTC;
			}
			if (name == "eth_eur")
			{
				return ExchangePairTypeEnum.ETH_EUR;
			}
			if (name == "btc_eur")
			{
				return ExchangePairTypeEnum.BTC_EUR;
			}
			if (name == "btc_eth")
			{
				return ExchangePairTypeEnum.ETH_BTC;
			}

			return ExchangePairTypeEnum.BTC_USD;
		}

		private string ResolveCurrencyNrToSymbol(int SymbolNr)
		{
			switch (SymbolNr)
			{
				case 1:
				{
					return "usd";
				}
				case 2:
				{
					return "eur";
				}
				case 3:
				{
					return "btc";
				}
				case 4:
				{
					return "eth";
				}
				default:
					break;
			}

			return "unknown";
		}
	}
}
