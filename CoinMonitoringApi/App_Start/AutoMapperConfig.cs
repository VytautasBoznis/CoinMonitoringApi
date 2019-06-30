using AutoMapper;
using AutoMapper.Configuration;
using CoinMonitoringApi.App_Start.Profiles;

namespace CoinMonitoringApi.App_Start
{
	public static class AutoMapperConfig
	{
		public static void Configure()
		{
			MapperConfigurationExpression configuration = new MapperConfigurationExpression();

			AddProfiles(configuration);

			Mapper.Initialize(configuration);
		}

		public static void AddProfiles(MapperConfigurationExpression configuration)
		{
			configuration.AddProfile<AccountProfile>();
			configuration.AddProfile<TradeProfile>();
		}
	}
}