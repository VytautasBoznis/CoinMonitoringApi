namespace CoinMonitoringPortalApi.Business.Database.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User_Balances
    {
        [Key]
        public int BalanceNr { get; set; }

        public int UserNr { get; set; }

        public int ExchangeType { get; set; }

        public int CurrencyType { get; set; }

        public decimal Value { get; set; }

        public virtual CurrencyType CurrencyType1 { get; set; }

        public virtual ExchangeType ExchangeType1 { get; set; }

        public virtual User_Users User_Users { get; set; }
    }
}
