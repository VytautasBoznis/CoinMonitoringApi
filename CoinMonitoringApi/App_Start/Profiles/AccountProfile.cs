using AutoMapper;
using CoinMonitoringPortalApi.Business.Database.Context;
using CoinMonitoringPortalApi.Data.Messages.Account;
using CoinMonitoringPortalApi.Data.Data.Account;

namespace CoinMonitoringApi.App_Start.Profiles
{
	public class AccountProfile : Profile
	{
		public AccountProfile()
		{
			CreateMap<RegisterRequest, User_Users>().ReverseMap();
			CreateMap<User_Users, User>().ReverseMap();
			CreateMap<User_Keys, AuthKey>().ReverseMap();
		}
	}
}