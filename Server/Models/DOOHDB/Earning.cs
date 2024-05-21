using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Earning", Schema = "dbo")]
    public partial class Earning
    {

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
            get;
            set;
        }

        [Key]
        [Required]
        public int EarningId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int ProviderId { get; set; }

        public Provider Provider { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int AnalyticId { get; set; }

        public Analytic Analytic { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int AdboardId { get; set; }

        public Adboard Adboard { get; set; }

        [ConcurrencyCheck]
        public int TotalDuration { get; set; }

        [ConcurrencyCheck]
        public decimal? RatePerSecond { get; set; }

        [ConcurrencyCheck]
        public decimal? TotalAmountBeforeTax { get; set; }

        [ConcurrencyCheck]
        public decimal? EarningPercent { get; set; }

        [ConcurrencyCheck]
        public decimal? EarningAmount { get; set; }
    }
}