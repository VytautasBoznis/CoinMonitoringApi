namespace CoinMonitoringPortalApi.Business.Database.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Trade_Trades
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Trade_Trades()
        {
            Trade_Criteria = new HashSet<Trade_Criteria>();
        }

        [Key]
        public int TradeNr { get; set; }

        public int UserNr { get; set; }

        public DateTime CreationDate { get; set; }

        public decimal Value { get; set; }

        public int TradeAction { get; set; }

        public int TradeStatus { get; set; }

        public DateTime? CompletionTime { get; set; }

        public int ExchangePairNr { get; set; }

        public virtual Exchange_Pairs Exchange_Pairs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Trade_Criteria> Trade_Criteria { get; set; }

        public virtual TradeActionType TradeActionType { get; set; }

        public virtual TradeStatusType TradeStatusType { get; set; }

        public virtual User_Users User_Users { get; set; }
    }
}
