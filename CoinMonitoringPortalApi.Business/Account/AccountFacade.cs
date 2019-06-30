using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoinMonitoringApi.Interfaces.Account;
using CoinMonitoringPortalApi.Business.Database.Context;
using CoinMonitoringPortalApi.Business.Database.Enums;
using CoinMonitoringPortalApi.Data.Data.Account;
using CoinMonitoringPortalApi.Data.Data.Trade;
using CoinMonitoringPortalApi.Data.Messages.Account;
using log4net;

namespace CoinMonitoringPortalApi.Business.Account
{
	public class AccountFacade: IAccountFacade
	{
		private DatabaseContext db;
		private static ILog _logger;
		protected ILog Logger => _logger;

		public AccountFacade()
		{
			db = new DatabaseContext();
			_logger = LogManager.GetLogger(GetType().Name);
		}

		public LoginResponse Login(LoginRequest request)
		{
			LoginResponse response = new LoginResponse
			{
				Success = true,
				Error = ""
			};

			User_Users user = db.User_Users.FirstOrDefault(u => u.UserName == request.UserName && u.Password == request.Password);

			if (user == null)
			{
				response.Success = false;
				response.Error = "Username or password is incorrect";
			}
			else
			{
				response.User = Mapper.Map<User>(user);

				#region CEX Balances
				List<User_Balances> allBalances = db.User_Balances.Where(b => b.ExchangeType == (int)ExchangeTypeEnum.Cex && b.UserNr == response.User.UserNr).ToList();
				List<PortfolioData> portfolioDatas = null;

				if (allBalances.Count > 0)
				{
					portfolioDatas = new List<PortfolioData>
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
				}
				
				#endregion

				#region Poloniex Balances

				allBalances = db.User_Balances.Where(b => b.ExchangeType == (int)ExchangeTypeEnum.Poloniex && b.UserNr == response.User.UserNr).ToList();

				if (allBalances.Count > 0)
				{
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
				}

				#endregion

				response.PortfolioDatas = portfolioDatas;
			}

			return response;
		}

		public RegisterResponse Register(RegisterRequest request)
		{
			User_Users user = Mapper.Map<User_Users>(request);

			RegisterResponse response = new RegisterResponse
			{
				Success = true,
				Error = ""
			};

			User_Users DbUser = db.User_Users.FirstOrDefault(u => u.Email == request.Email && u.UserName == request.UserName);

			if (DbUser != null)
			{
				response.Success = false;
				response.Error = "A user with that email or username already exists";
				return response;
			}

			try
			{
				db.User_Users.Add(user);
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

		public UpdateUserResponse UpdateUser(UpdateUserRequest request)
		{
			UpdateUserResponse response = new UpdateUserResponse
			{
				Success = true,
				Error = ""
			};

			User_Users DbUser = db.User_Users.FirstOrDefault(u => u.UserNr == request.UserNr);

			if (DbUser == null)
			{
				response.Success = false;
				response.Error = "No user found";
				return response;
			}

			try
			{
				DbUser.Email = request.Email;
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

		public ChangeUserPasswordResponse ChangePassword(ChangeUserPasswordRequest request)
		{
			ChangeUserPasswordResponse response = new ChangeUserPasswordResponse
			{
				Success = true,
				Error = ""
			};

			User_Users DbUser = db.User_Users.FirstOrDefault(u => u.UserNr == request.UserNr);

			if (DbUser == null)
			{
				response.Success = false;
				response.Error = "No user found";
				return response;
			}

			try
			{
				DbUser.Password = request.NewPassword;
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

		public AuthKeysResponse GetAuthKeys(AuthKeysRequest request)
		{
			AuthKeysResponse response = new AuthKeysResponse
			{
				Success = true,
				Error = ""
			};

			try
			{
				List<User_Keys> keys = db.User_Keys.Where(k => k.UserNr == request.UserNr).ToList();
				response.AuthKeys = Mapper.Map<List<AuthKey>>(keys);
			}
			catch(Exception e)
			{
				_logger.ErrorFormat($"error happend error: {e.Message} stacktrace: {e.StackTrace}");
				response.Success = false;
				response.Error = "An error occured in user creation please check logs for more details";
			}
			
			return response;
		}

		public CreateAuthKeyResponse CreateAuthKey(CreateAuthKeyRequest request)
		{
			CreateAuthKeyResponse response = new CreateAuthKeyResponse
			{
				Success = true,
				Error = ""
			};

			User_Keys key = Mapper.Map<User_Keys>(request.AuthKey);

			User_Keys DbKey = db.User_Keys.FirstOrDefault(k => k.UserNr == key.UserNr && k.ExchangeType == key.ExchangeType);

			if (DbKey != null)
			{
				response.Success = false;
				response.Error = "A key for that exchange already exists";
				return response;
			}

			try
			{
				db.User_Keys.Add(key);
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

		public DeleteAuthKeyResponse DeleteAuthKey(DeleteAuthKeyRequest request)
		{
			DeleteAuthKeyResponse response = new DeleteAuthKeyResponse
			{
				Success = true,
				Error = ""
			};

			User_Keys DbKey = db.User_Keys.FirstOrDefault(k => k.UserNr == request.UserNr && k.KeyNr == request.KeyNr);

			if (DbKey == null)
			{
				response.Success = false;
				response.Error = "No such key found";
				return response;
			}

			try
			{
				db.User_Keys.Remove(DbKey);
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
	}
}
