namespace CoinMonitoringPortalApi.Business.Database.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Exchange_Pairs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Exchange_Pairs()
        {
            Trade_Trades = new HashSet<Trade_Trades>();
        }

        [Key]
        public int PairNr { get; set; }

        public int ExchangeType { get; set; }

        [Required]
        [StringLength(256)]
        public string Symbol1 { get; set; }

        [Required]
        [StringLength(256)]
        public string Symbol2 { get; set; }

        public virtual ExchangeType ExchangeType1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Trade_Trades> Trade_Trades { get; set; }
    }
}
