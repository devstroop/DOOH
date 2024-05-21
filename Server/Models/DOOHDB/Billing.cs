using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Billing", Schema = "dbo")]
    public partial class Billing
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
        public int BillingId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string UserId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int AnalyticId { get; set; }

        public Analytic Analytic { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int AdvertisementId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int CampaignId { get; set; }

        [ConcurrencyCheck]
        public int? TotalDuration { get; set; }

        [ConcurrencyCheck]
        public decimal? RatePerSecond { get; set; }

        [ConcurrencyCheck]
        public decimal? Taxable { get; set; }

        [ConcurrencyCheck]
        public decimal? TaxRate { get; set; }

        [ConcurrencyCheck]
        public decimal? TaxAmount { get; set; }

        [ConcurrencyCheck]
        public decimal? Total { get; set; }

        [ConcurrencyCheck]
        public decimal? RoundOff { get; set; }

        [ConcurrencyCheck]
        public decimal? Payable { get; set; }

        [ConcurrencyCheck]
        public int? TaxId { get; set; }

        public Tax Tax { get; set; }
    }
}