namespace CoinMonitoringPortalApi.Business.Database.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User_Keys
    {
        [Key]
        public int KeyNr { get; set; }

        public int UserNr { get; set; }

        public int ExchangeType { get; set; }

        [Required]
        [StringLength(256)]
        public string KeyValue { get; set; }

        [Required]
        [StringLength(256)]
        public string SecretValue { get; set; }

        public virtual ExchangeType ExchangeType1 { get; set; }

        public virtual User_Users User_Users { get; set; }
    }
}
