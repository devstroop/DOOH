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
        [Key]
        [Required]
        public int BillingId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int AnalyticId { get; set; }

        public Analytic Analytic { get; set; }

        [Required]
        public int AdvertisementId { get; set; }

        [Required]
        public int CampaignId { get; set; }

        public int? TotalDuration { get; set; }

        public decimal? RatePerSecond { get; set; }

        public decimal? Taxable { get; set; }

        public decimal? TaxRate { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? Total { get; set; }

        public decimal? RoundOff { get; set; }

        public decimal? Payable { get; set; }

        public int? TaxId { get; set; }

        public Tax Tax { get; set; }
    }
}