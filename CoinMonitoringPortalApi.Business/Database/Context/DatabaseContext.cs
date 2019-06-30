namespace CoinMonitoringPortalApi.Business.Database.Context
{
	using System.Data.Entity;

	public partial class DatabaseContext : DbContext
	{
		public DatabaseContext()
			: base("name=CoinMonitoring")
		{
		}

		public virtual DbSet<CriteriaValueType> CriteriaValueTypes { get; set; }
		public virtual DbSet<CurrencyType> CurrencyTypes { get; set; }
		public virtual DbSet<EcoIndexType> EcoIndexTypes { get; set; }
		public virtual DbSet<Exchange_Pairs> Exchange_Pairs { get; set; }
		public virtual DbSet<ExchangeType> ExchangeTypes { get; set; }
		public virtual DbSet<Trade_Criteria> Trade_Criteria { get; set; }
		public virtual DbSet<Trade_Trades> Trade_Trades { get; set; }
		public virtual DbSet<TradeActionType> TradeActionTypes { get; set; }
		public virtual DbSet<TradeStatusType> TradeStatusTypes { get; set; }
		public virtual DbSet<User_Balances> User_Balances { get; set; }
		public virtual DbSet<User_Keys> User_Keys { get; set; }
		public virtual DbSet<User_Users> User_Users { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CriteriaValueType>()
				.HasMany(e => e.Trade_Criteria)
				.WithRequired(e => e.CriteriaValueType1)
				.HasForeignKey(e => e.CriteriaValueType)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<CurrencyType>()
				.HasMany(e => e.User_Balances)
				.WithRequired(e => e.CurrencyType1)
				.HasForeignKey(e => e.CurrencyType)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<EcoIndexType>()
				.HasMany(e => e.Trade_Criteria)
				.WithRequired(e => e.EcoIndexType1)
				.HasForeignKey(e => e.EcoIndexType)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Exchange_Pairs>()
				.HasMany(e => e.Trade_Trades)
				.WithRequired(e => e.Exchange_Pairs)
				.HasForeignKey(e => e.ExchangePairNr)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ExchangeType>()
				.HasMany(e => e.Exchange_Pairs)
				.WithRequired(e => e.ExchangeType1)
				.HasForeignKey(e => e.ExchangeType)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ExchangeType>()
				.HasMany(e => e.User_Balances)
				.WithRequired(e => e.ExchangeType1)
				.HasForeignKey(e => e.ExchangeType)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ExchangeType>()
				.HasMany(e => e.User_Keys)
				.WithRequired(e => e.ExchangeType1)
				.HasForeignKey(e => e.ExchangeType)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Trade_Criteria>()
				.Property(e => e.Value)
				.HasPrecision(18, 9);

			modelBuilder.Entity<Trade_Criteria>()
				.Property(e => e.Weight)
				.HasPrecision(18, 5);

			modelBuilder.Entity<Trade_Trades>()
				.Property(e => e.Value)
				.HasPrecision(18, 9);

			modelBuilder.Entity<Trade_Trades>()
				.HasMany(e => e.Trade_Criteria)
				.WithRequired(e => e.Trade_Trades)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<TradeActionType>()
				.HasMany(e => e.Trade_Trades)
				.WithRequired(e => e.TradeActionType)
				.HasForeignKey(e => e.TradeAction)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<TradeStatusType>()
				.HasMany(e => e.Trade_Trades)
				.WithRequired(e => e.TradeStatusType)
				.HasForeignKey(e => e.TradeStatus)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<User_Balances>()
				.Property(e => e.Value)
				.HasPrecision(18, 9);

			modelBuilder.Entity<User_Users>()
				.HasMany(e => e.Trade_Trades)
				.WithRequired(e => e.User_Users)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<User_Users>()
				.HasMany(e => e.User_Balances)
				.WithRequired(e => e.User_Users)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<User_Users>()
				.HasMany(e => e.User_Keys)
				.WithRequired(e => e.User_Users)
				.WillCascadeOnDelete(false);
		}
	}
}
