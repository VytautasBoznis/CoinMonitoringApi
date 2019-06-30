namespace CoinMonitoringPortalApi.Business.Database.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Trade_Criteria
    {
        [Key]
        public int CriteriaNr { get; set; }

        public int TradeNr { get; set; }

        public int EcoIndexType { get; set; }

        public int CriteriaValueType { get; set; }

        public decimal Value { get; set; }

        public decimal Weight { get; set; }

        public virtual CriteriaValueType CriteriaValueType1 { get; set; }

        public virtual EcoIndexType EcoIndexType1 { get; set; }

        public virtual Trade_Trades Trade_Trades { get; set; }
    }
}
